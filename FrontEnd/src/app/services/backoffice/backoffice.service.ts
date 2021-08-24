import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from '../auth/auth.service';
import { ActualizarObraPendiente, Artista } from '../../models';

@Injectable({
  providedIn: 'root'
})
export class BackofficeService {

  constructor( private http: HttpClient, private auth: AuthService ) {  }

  async obtenerObrasPendientes(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/Obra/Pendiente`, { headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
          return { status: e.status, message: e.message };
      })
    .catch( e => 
      {        
        return { status: e.status };   
      }); 
  }

  async actualizarObrasPendientes( model: ActualizarObraPendiente ): Promise<any>
  { 
    return this.http.put<any>(`${environment.apiURL}/Obra/ActualizarObraPendiente`, model , { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        //console.log(e)
        return { status: e.status };   
      })
    .catch( e => 
      {     
        //console.log(e)
        return { status: 404, message: e };   
      }); 
  }

  async obtenerArtistasPendientes()
  { 
    return this.http.get<any>(`${environment.apiURL}/Usuarios/ArtistasPendientes`, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        return { status: 200, message: e };   
      })
    .catch( e => 
      {     
        return { status: e.status, message: e };   
      });
  }

  async aprobarArtista( id: Artista ): Promise<any>
  {  
    return this.http.put<any>(`${environment.apiURL}/Usuarios/AprobarArtista`, id , { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        return { status: e.status };   
      })
    .catch( e => 
      {     
        return { status: e.status, message: e };   
      }); 
  }

  async rechazarArtista( model: Artista ): Promise<any>
  {
    return this.http.put<any>(`${environment.apiURL}/Usuarios/RechazarArtista`, model , { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        return { status: e.status };   
      })
    .catch( e => 
      {     
        return { status: e.status, message: e };   
      }); 
  }

  async obtenerTickets(): Promise<any>
  {  
    return this.http.get<any>(`${environment.apiURL}/PerfilUsuario/TicketsPendientes`, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {
        //console.log(e);
        return { status: 200, message: e.tickets };   
      })
    .catch( e => 
      {  
        return { status: e.status, message: e };   
      }); 
  }

  async solicitudCambiosArtista( data: any ): Promise<any>
  { 
    return this.http.post<any>(`${environment.apiURL}/PerfilUsuario/RechazarCambio`, data, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {        
        //console.log(e)
        return { status: e.status, message: null };   
      })
    .catch( e => 
      {  
        console.log(e)
        return { status: e.status, message: e };   
      }); 
  }

}
