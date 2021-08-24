import { Component, OnInit } from '@angular/core';
import { BackofficeService } from '../services/backoffice/backoffice.service';

import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-backoffice',
  templateUrl: './backoffice.component.html',
  styleUrls: ['./backoffice.component.scss']
})
export class BackofficeComponent implements OnInit {

  obrasPendientes: [] = [];
  artistasPendientes: [] = [];
  ticketPendientes: [] = [];

  _mensajes: boolean = false;
  _publicaciones: boolean = false;   
  _registros: boolean = false;   
  _tickets: boolean = false;   

  constructor( private backOfficeService: BackofficeService, private route: Router, private auth: AuthService ) { }

  ngOnInit()
  {
    this.obtenerObrasPendientes();
    this.obtenerArtistasPendientes();
    this.ticketsPendientes();
  } 

  actualizar()
  {
    this.obtenerObrasPendientes();
    this.obtenerArtistasPendientes();
    this.ticketsPendientes();
  }

  Mensajes()
  {
    this._mensajes = true;  
    this._publicaciones = false;
    this._registros = false;
    this._tickets = false;  
  }

  publicaciones()
  {
    this._publicaciones = true;
    this._registros = false;
    this._tickets = false;
  }

  registros()
  {
    this._registros = true;
    this._publicaciones = false;
    this._tickets = false;
  }

  tickets()
  {
    this._tickets = true;
    this._registros = false;
    this._publicaciones = false;    
  }

  obtenerObrasPendientes()
  {  
      this.backOfficeService.obtenerObrasPendientes()
      .then( e => 
      {         
        //console.log(e)
        if ( e.status == 200 ) 
        {
          this.obrasPendientes = e.message;
          //console.log( this.obrasPendientes )
          return;                
        }

        if (e.status == 403 || e.status == 401) 
        {
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );     
          return;
        }                
        
      });  
  }

  obtenerArtistasPendientes()
  {
    this.backOfficeService.obtenerArtistasPendientes()
    .then( e => 
      {
        //console.log(e)
        if ( e.status == 200 ) 
        {
          this.artistasPendientes = e.message;
          return;                
        }

        if (e.status == 403 || e.status == 401) 
        {
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );  
          return;
        }
        
      })
  }

  ticketsPendientes()
  {
    this.backOfficeService.obtenerTickets()
    .then( e => 
      {
        if ( e.status == 200 ) 
        {
          this.ticketPendientes = e.message;
          return;                
        }
        if (e.status == 403 || e.status == 401) 
        {
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );  
          return;
        }
      });
  }

}
