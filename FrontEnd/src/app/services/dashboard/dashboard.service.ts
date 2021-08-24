import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NuevaPublicacion } from '../../models/index';

import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {  
  
  constructor( private http: HttpClient, private auth: AuthService ) { }

  async getInfoUser( id: any ): Promise<any>
  { 
    let body = {
      "Id": id
    };

    return this.http.post<any>(`${environment.apiURL}/Usuarios/InfoUser`, body ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async getObrasUser(id: any): Promise<any>
  {
    let body = {
      "Id": id
    };

    return this.http.post<any>(`${environment.apiURL}/Obra/GetByUser`, body, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      }); 
  }

  async getInfoArtista(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/Usuarios/InfoArtista`, { headers: await this.auth.Header() }).toPromise()
      .then(e => {       
       return { status: 200, message: e };
      })
      .catch(e => {        
        return { status: e.status, message: e.error };
      });    
  }

  async getObrasArtista(): Promise<any>
  {  
    return this.http.get<any>(`${environment.apiURL}/Obra/GetByArtista`, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: 200, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      }); 
  }  

  async nuevaPublicacion( model : NuevaPublicacion ): Promise<any>
  {  
    return this.http.post<any>(`${environment.apiURL}/Obra/Crear`, model, { headers: await this.auth.Header() }).toPromise()
      .then( e => {
       console.log(e);
       return { status: e.status };
      })
      .catch(e => {
        console.log(e);
        return { status: e.status, message: e };
      }); 
  }

}
