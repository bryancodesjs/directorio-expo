import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr'; 
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class MensajeriaService {
  token: any = "";
  private lastIdConexion: string = '';
  private sharedObj = new Subject<any>();
  model = {
    'idConexion': null,
    'token': ''
  }

  private connection: any = new signalR.HubConnectionBuilder().withUrl( environment.urlSockets ) 
  .configureLogging(signalR.LogLevel.Information)
  .build();

  constructor( private http: HttpClient, private auth: AuthService ) 
  {
    this.connection.connection.onclose( async () => 
    {
      console.log( "Desconectado");
      await this.start();
    });

    this.connection.on("Update", () => 
    { 
      //console.log("Nuevos mensajes");
      this.sharedObj.next("Nuevo_Mensaje");
    });
    
    this.start();   
  }

  public async start() 
  {
    try 
    {
      await this.connection.start();

      if (this.connection.connectionState == "Connected")
      {  
        this.Conectado();
      }
      //console.log( this.connection.connection );
    } 
    catch (err) 
    {
      console.log(err);
      //setTimeout(() => this.start(), 5000);
    } 
  }

  public retrieveMappedObject(): Observable<any> {
    return this.sharedObj.asObservable();
  }

  async tipoSolicitud(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/Mensajeria/tipoSolicitud`, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status, message: e.message };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async enviarMensaje( model: any ): Promise<any>
  {  
    return this.http.post<any>(`${environment.apiURL}/Mensajeria/EnviarMensaje`, model, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async chats(): Promise<any>
  { 
    return this.http.get<any>(`${environment.apiURL}/Mensajeria/Chats`, { headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status, message: e };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async mensajeLeido( model: any ): Promise<any>
  { 
    return this.http.put<any>(`${environment.apiURL}/Mensajeria/MensajeLeido`,model ,{ headers: await this.auth.Header() }).toPromise()
      .then(e => {
       
       return { status: e.status };
      })
      .catch(e => {
        //console.log(e.status);
        return { status: e.status, message: e.error };
      });    
  }

  async Conectado(): Promise<any>
  { 
     this.auth.getToken().then( async e => 
      {      
        this.token = e;
        this.model.idConexion = this.connection.connectionId;        
        this.model.token = this.token;
        this.http.post<any>(`${environment.apiURL}/Mensajeria/Conectado`, this.model ,{ headers: await this.auth.Header() }).toPromise()
        .then(e => {
          if (e.status == 200) 
          {
            //this.model.lastIdConexion = e.lastIdConexion;
            console.log(e);      
          }          
        })
        .catch( e => 
          {
            console.log(e);        
          });    
      });
  }

  async desConectado(): Promise<any>
  { 
     this.auth.getToken().then( async e => 
      {      
        this.token = e;                
        this.model.token = this.token;
        this.http.post<any>(`${environment.apiURL}/Mensajeria/Desconectado`, this.model ,{ headers: await this.auth.Header() }).toPromise()
        .then(e => {
          if (e.status == 200) 
          {
            //this.model.lastIdConexion = e.lastIdConexion;
            // console.log(e);   
            console.log("Usuario desconectado");      
          }          
        })
        .catch( e => 
          {
            console.log(e);        
          });    
      });
  }

}
