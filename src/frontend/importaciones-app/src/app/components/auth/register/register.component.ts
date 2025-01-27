import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;
  userValidated = false;
  userName = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.registerForm = this.fb.group({
      cedula: ['', [Validators.required]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
      ]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validator: this.passwordMatchValidator
    });
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password')?.value === g.get('confirmPassword')?.value
      ? null : { 'mismatch': true };
  }

  validateCedula(): void {
    const cedula = this.registerForm.get('cedula')?.value;
    if (!cedula) {
      this.snackBar.open('Por favor ingrese una cédula', 'Cerrar', { duration: 3000 });
      return;
    }

    this.isLoading = true;
    this.authService.validateUser(cedula).subscribe({
      next: (response) => {
        if (response.isValid) {
          this.userValidated = true;
          this.userName = response.name;
          this.snackBar.open('Usuario validado exitosamente', 'Cerrar', { duration: 3000 });
        } else {
          this.snackBar.open(response.message, 'Cerrar', { duration: 3000 });
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.snackBar.open('Error al validar el usuario: ' + error.message, 'Cerrar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid && this.userValidated) {
      this.isLoading = true;
      this.authService.register(this.registerForm.value).subscribe({
        next: () => {
          this.snackBar.open('Registro exitoso. Por favor inicie sesión.', 'Cerrar', { duration: 3000 });
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.snackBar.open('Error en el registro: ' + error.message, 'Cerrar', { duration: 3000 });
          this.isLoading = false;
        }
      });
    }
  }

  getPasswordErrorMessage(): string {
    const control = this.registerForm.get('password');
    if (control?.hasError('required')) {
      return 'La contraseña es requerida';
    }
    if (control?.hasError('minlength')) {
      return 'La contraseña debe tener al menos 8 caracteres';
    }
    if (control?.hasError('pattern')) {
      return 'La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial';
    }
    return '';
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}