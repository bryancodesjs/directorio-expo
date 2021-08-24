import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContadorVisitasService {

  constructor( private auth: AuthService, private http: HttpClient ) { }

  async nuevaVisita( id: number )
  {  
    return this.http.post<any>(`${environment.apiURL}/PerfilUsuario/NuevaVisita`, id ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
          return { status: e.status, message: null };
      })
    .catch( e => 
      {        
        return { status: e.status, message: e.error.message };   
      }); 
  }
  
}
