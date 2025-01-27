import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';
import { CompanyService, Company } from '../../services/company.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  userName: string = '';
  selectedCompany: string = '';
  companies: Company[] = [];
  isLoading = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private companyService: CompanyService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadUserData();
    this.loadCompanies();
  }

  private loadUserData(): void {
    const userData = this.authService.currentUserValue;
    if (userData) {
      this.userName = userData.name;
    }
  }

  private loadCompanies(): void {
    this.isLoading = true;
    
    // Primero cargar todas las compañías
    this.companyService.getCompanies().subscribe({
      next: (companies) => {
        this.companies = companies;
        
        // Verificar si ya hay una compañía seleccionada
        const currentCompany = this.companyService.getCurrentCompany();
        if (currentCompany) {
          this.selectedCompany = currentCompany.id;
        } else {
          // Si no hay compañía seleccionada, cargar la predeterminada
          this.loadDefaultCompany();
        }
      },
      error: (error) => {
        this.snackBar.open('Error al cargar las compañías', 'Cerrar', {
          duration: 3000
        });
        this.isLoading = false;
      }
    });
  }

  private loadDefaultCompany(): void {
    this.companyService.getDefaultCompany().subscribe({
      next: (company) => {
        this.selectedCompany = company.id;
      },
      error: (error) => {
        this.snackBar.open('Error al cargar la compañía predeterminada', 'Cerrar', {
          duration: 3000
        });
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onCompanyChange(event: any): void {
    const selectedId = event.value;
    const company = this.companies.find(c => c.id === selectedId);
    if (company) {
      this.companyService.setSelectedCompany(company);
      this.snackBar.open(`Compañía seleccionada: ${company.name}`, 'Cerrar', {
        duration: 3000
      });
    }
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