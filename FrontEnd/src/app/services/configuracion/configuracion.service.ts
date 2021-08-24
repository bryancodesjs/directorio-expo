import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConfiguracionService {

  constructor( private auth: AuthService, private http: HttpClient ) { }

  async cambiarClave( model: any )
  {   

    return this.http.post<any>(`${environment.apiURL}/PerfilUsuario/CambiarClave`, model ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
          return { status: e.status, message: null };
      })
    .catch( e => 
      {        
        return { status: e.status, message: e.error.message };   
      }); 
  }

  async ActualizarPerfil( model: any )
  {  
    return this.http.put<any>(`${environment.apiURL}/PerfilUsuario/ActualizarPerfil`, model ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
          return { status: e.status, message: null };
      })
    .catch( e => 
      {        
        return { status: e.status, message: e };   
      });
  }


}
