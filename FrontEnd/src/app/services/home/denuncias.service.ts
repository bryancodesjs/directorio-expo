import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { denuncias as denunciasModel } from '../../models';

@Injectable({
  providedIn: 'root'
})
export class DenunciasService {

  constructor( private auth: AuthService, private http: HttpClient ) { }

  async denunciarObra( data: denunciasModel )
  { 
    return this.http.post<any>(`${environment.apiURL}/Obra/DenunciarObra`, data ,{ headers: await this.auth.Header() }).toPromise() 
    .then( e =>      
      { 
        //console.log(e);  
        return { status: e.status, message: null };
      })
    .catch( e => 
      {      
        //console.log(e);  
        return { status: e.status, message: e.error.message };   
      }); 
  }

}
