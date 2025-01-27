import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface Company {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private readonly SELECTED_COMPANY_KEY = 'selected_company';
  private selectedCompanySubject: BehaviorSubject<Company | null>;
  public selectedCompany: Observable<Company | null>;

  constructor(private http: HttpClient) {
    this.selectedCompanySubject = new BehaviorSubject<Company | null>(
      this.getStoredCompany()
    );
    this.selectedCompany = this.selectedCompanySubject.asObservable();
  }

  getCompanies(): Observable<Company[]> {
    return this.http.get<Company[]>(`${environment.apiUrl}/company`);
  }

  getDefaultCompany(): Observable<Company> {
    return this.http.get<Company>(`${environment.apiUrl}/company/default`)
      .pipe(
        tap(company => {
          this.setSelectedCompany(company);
        })
      );
  }

  setSelectedCompany(company: Company): void {
    localStorage.setItem(this.SELECTED_COMPANY_KEY, JSON.stringify(company));
    this.selectedCompanySubject.next(company);
  }

  private getStoredCompany(): Company | null {
    const storedCompany = localStorage.getItem(this.SELECTED_COMPANY_KEY);
    return storedCompany ? JSON.parse(storedCompany) : null;
  }

  getCurrentCompany(): Company | null {
    return this.selectedCompanySubject.value;
  }
}