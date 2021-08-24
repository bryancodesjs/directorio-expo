import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class VisitasService {

  constructor( private http: HttpClient, private auth: AuthService ) { }

  async VerificarVisita( _id: number )
  {    
    // FORMATO DE FECHA
    //const hoy = new Date();
    //const _fecha = (hoy.getMonth() + 1) + '-' + hoy.getDate() + '-' + hoy.getFullYear() + ' ' + hoy.getHours() + ':' + (hoy.getMinutes() < 10 ? '0' + hoy.getMinutes() : hoy.getMinutes()) + ':' + hoy.getSeconds();
    const _fecha = moment().format('LLL');
    //console.log(_fecha)
    // SINO EXISTE UNA LISTA EN LOCAL STORAGE, SE CREA UNA
    if ( await this.ListaVisitas() == null) 
    {
      let arrayVisitas: any[] = [];      
     
      const objeto = { id: _id, fecha: _fecha };
      
      arrayVisitas.push( objeto );
      await this.GuardarLista( arrayVisitas ); 

      this.SumarVisita(_id);     
    }
    else
    {   
      // SE OBTIENE LA LISTA DE VISITAS ALMACENADA EN LOCAL STORAGE
      const array: any = await this.ListaVisitas();      
      // SE CONVIERTE A FORMATO JSON
      let lista: any[] = await JSON.parse(array);      
      // SE BUSCA EN LA LISTA DEL LOCAL STORAGE ALGUN MATCH CON EL ID DE ENTRADA
      const existe = lista.find( e => e.id == _id ) != undefined ? lista.find( e => e.id == _id ) : false;      
      
      // EVALUA SI EXISTE EN LA LISTA DE VISITAS ALMACENADA EN LOCAL STORAGE
      if ( existe != false ) 
      {  
        //console.log(existe.fecha)
        const diferencia = moment(_fecha,'MMMM Do YYYY, h:mm:ss a').diff(moment(existe.fecha,'MMMM Do YYYY, h:mm:ss a'), 'minutes');

        //console.log(moment(_fecha))

        // SI LA DIFERENCIA ES MAYOR A 25 MINUTOS, CUENTA COMO VISITA
        if ( diferencia > 25 ) 
        {
          // SE SUMA LA VISITA
          this.SumarVisita( _id );

          // SE ACTUALIZA LA LISTA DE VISITAS
          lista = lista.filter(item => item.id !== _id);
          // SE CREA UN NUEVO OBJETO 
          const nuevoObjeto = { id: _id, fecha: _fecha };
          // SE AGREGA A LA LISTA EL NUEVO OBJETO
          lista.push( nuevoObjeto);
          
          await this.ActualizarLista(lista);

        }
        // SI LA DIFERENCIA ES MENOS A 25 MINUTOS, NO CUENTA COMO VISITA
        else
        {
          console.log( "Solo tiene de diferencia", diferencia );
        }
        
      }
      // SI NO EXISTE EN LOCAL STORAGE SE AGREGA A LA LISTA
      else
      {
      const objeto = { id: _id, fecha: _fecha };
      lista.push( objeto );
      await this.GuardarLista( lista );   
      // POR TERMINAR   
      this.SumarVisita( _id );
      } 
    }
  }

  async ListaVisitas()
  {    
    return await localStorage.getItem( "visitas" );    
  }  

  async GuardarLista( array: any )
  {        
    await localStorage.setItem( "visitas", JSON.stringify(array) );    
  }

  async ActualizarLista( array: any )
  {     
    await localStorage.removeItem("visitas");   
    await localStorage.setItem( "visitas", JSON.stringify(array) );    
  }

  async SumarVisita( id: number )
  {  
    const body = {
      "Id": id
    };

    return this.http.post<any>(`${environment.apiURL}/Obra/NuevaVisita`, body, { headers: await this.auth.Header() }).toPromise() 
    .then( e => 
      {        
        if (e.status === 200) 
        {
          console.log( "Visita sumada a", id );
          return { status: e.status, message: e.message } 
        }
        else
        {
          return { status: e.status, message: e.message };
        }
        
      })
    .catch( e => 
      {    
        console.log( e);    
        return { status: e.status, message: e.error };
      }); 
  }

}
