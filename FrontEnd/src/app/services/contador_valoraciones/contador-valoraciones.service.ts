import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContadorValoracionesService {

  constructor( private auth: AuthService, private http: HttpClient ) { }

  async nuevaValoracion( id: number )
  {
    const body = {
      "Id": id
    };

    return this.http.post<any>(`${environment.apiURL}/Obra/NuevaValoracion`, body ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
        //console.log(e)
          return { status: e.status, message: null };
      })
    .catch( e => 
      {        
        //console.log(e)
        return { status: e.status, message: e.error.message };   
      }); 
  }

  async revisarValoracion( id: number )
  {  
    const body = {
      "Id": id
    };

    return this.http.post<any>(`${environment.apiURL}/Obra/RevisarValoracion`, body ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
        //console.log(e)
        return { status: e.message };
      })
    .catch( e => 
      {        
        //console.log(e)
        return { status: e.status, message: e.message };   
      }); 
  }

}
