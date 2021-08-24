import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class GestionUsuariosService {

  constructor( private http: HttpClient, private auth: AuthService ) { }

  async Usuarios(): Promise<any>
  {   
    return this.http.get<any>(`${environment.apiURL}/AdministracionUser` ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async Habilitar( model: any ): Promise<any>
  {
    return this.http.put<any>(`${environment.apiURL}/AdministracionUser/Habilitar`, model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async Deshabilitar( model: any ): Promise<any>
  { 
    return this.http.put<any>(`${environment.apiURL}/AdministracionUser/Deshabilitar`, model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async Permisos( model: any ): Promise<any>
  {
    return this.http.post<any>(`${environment.apiURL}/AdministracionUser/Permisos`, model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async agregarPermisos( model: any ): Promise<any>
  { 
    return this.http.post<any>(`${environment.apiURL}/AdministracionUser/AgregarPermiso`, model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async quitarPermisos( model: any ): Promise<any>
  {  
    return this.http.post<any>(`${environment.apiURL}/AdministracionUser/QuitarPermiso`, model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async Check(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/AdministracionUser/Check`, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e.message };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

}
