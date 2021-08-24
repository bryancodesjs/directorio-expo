import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../../../environments/environment';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor( private http: HttpClient, private auth: AuthService ) { }  

  async obtenerGaleria( oderBy: string ): Promise<any>
  {  
    let body = {
      "Filtro": oderBy
    };

    return this.http.post<any>(`${environment.apiURL}/Obra/Get`, body, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        //console.log(e)
        return { status: e.codeStatus, message: e.message }; 
      })
    .catch( e => 
      {      
        //console.log(e)  
        return { status: e.status, message: e.error };
      }); 
  }

  async obtenerViolaciones(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/Obra/Violaciones`, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        return { status: e.codeStatus, message: e.message }         
      })
    .catch( e => 
      {        
        return { status: e.status, message: e.error };
      }); 
  }

}
