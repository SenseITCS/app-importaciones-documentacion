import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';

interface Company {
  id: string;
  name: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  userName: string = '';
  selectedCompany: string = '';
  companies: Company[] = [
    { id: '1', name: 'Compañía 1' },
    { id: '2', name: 'Compañía 2' },
    { id: '3', name: 'Compañía 3' }
  ];

  constructor(
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const userData = this.authService.currentUserValue;
    if (userData) {
      this.userName = userData.name;
    }
  }

  onCompanyChange(event: any): void {
    this.selectedCompany = event.value;
    // Aquí puedes agregar la lógica para manejar el cambio de compañía
    this.snackBar.open(`Compañía seleccionada: ${this.selectedCompany}`, 'Cerrar', {
      duration: 3000
    });
  }

  navigateToDocuments(): void {
    if (!this.selectedCompany) {
      this.snackBar.open('Por favor seleccione una compañía', 'Cerrar', {
        duration: 3000
      });
      return;
    }
    this.router.navigate(['/documents']);
  }

  navigateToPOPlan(): void {
    if (!this.selectedCompany) {
      this.snackBar.open('Por favor seleccione una compañía', 'Cerrar', {
        duration: 3000
      });
      return;
    }
    this.router.navigate(['/po-plan']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}