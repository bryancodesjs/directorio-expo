import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {  
  constructor( private http: HttpClient, private router: Router ){ }

  async setToken( token: string )
  {    
    await localStorage.setItem("token", token);    
  }   

  async getToken(){
    return await localStorage.getItem("token");
  }

  async authorized(): Promise<boolean>
  {
      if ( await this.getToken() != null && await this.getToken() != '' ) 
      {        
        return true;
      }
      else
      {
        return false;
      }
  }

  async Acceso(): Promise<any>
  {
    return this.http.get<any>(`${environment.apiURL}/Login/Acceso`, { headers: await this.Header() }).toPromise() 
    .then( e =>      
      {           
          return { status: e.status, message: e.message, usuario: e.usuario, permisoArtis_Empre: e.permisoProf_Empre };
      })
    .catch( e => 
      {        
        return { status: e.status };   
      }); 
  } 

  async Header()
  {
    let headers = 
    {
      "Authorization": `bearer ${ await this.getToken() }`,      
      "Content-Type": "application/json"    
    };

    return headers;
  }

  redireccionar( returnUrl: any )
  {
    const valorTotal =  returnUrl.length;
    const indice = returnUrl.indexOf('#') + 1;
    const ubicacion = document.location.hash.substring(indice + 1, valorTotal);

    //document.location.href = `/login?returnUrl=${ubicacion}`
    this.router.navigate(['/login'], { queryParams: { returnUrl: ubicacion } });
  }

  sesionExpirada( returnUrl: any )
  {
    const valorTotal =  returnUrl.length;
    const indice = returnUrl.indexOf('#') + 1;
    const ubicacion = document.location.hash.substring(indice + 1, valorTotal);

    //document.location.href = `/login?returnUrl=${ubicacion}`
    this.router.navigate(['/login'], { queryParams: { returnUrl: ubicacion, se: true } });
  }

  logOut()
  {    
    localStorage.removeItem("token");    
  }

}
