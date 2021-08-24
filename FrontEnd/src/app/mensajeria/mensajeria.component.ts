import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MensajeriaService } from '../services/mensajeria/mensajeria.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
declare var $: any;

@Component({
  selector: 'app-mensajeria',
  templateUrl: './mensajeria.component.html',
  styleUrls: ['./mensajeria.component.scss']
})
export class MensajeriaComponent implements OnInit { 
  @Input() backOffice: boolean = false;

  Chats: any = [];
  chatSelect: boolean = false;
  historialMensajes: any = [];
  userId: number = 0;
  _submitMensaje: boolean = false;
  ultimoChat: number = 0;

  constructor( private route: ActivatedRoute, private mensajeriaService: MensajeriaService, private auth: AuthService ) 
  { 
  }

  get m() { return this.mensajeForm.controls; }  

  mensajeForm = new FormGroup({
    Id_receptor: new FormControl(''), 
    detalles: new FormControl('', Validators.required)  
  });

  ngOnInit(): void 
  {
    this.route.params.subscribe( (params) => 
    {
      const param = params['id'] != undefined ? true : false;
      //console.log(param);
      this.chats();
    });    

    this.mensajeriaService.retrieveMappedObject().subscribe( (e) => 
    {
      this.chats();
    })
  } 

  p()
  {
    console.log("Mensaje")
  }

  chats()
  {
    this.mensajeriaService.chats()
    .then( e => 
      {
        if (e.status == 200) 
        {
          //console.log(e.message.chats)
          this.Chats = e.message.chats;          
          this.userId = e.message.userId;          
          if (this.ultimoChat > 0) 
          {
            this.Chats.forEach( (e: any) => {
              if(e.idChat == this.ultimoChat)
              {
                this.verMensajes( e, false );
              }
            });
          }
        }

        if (e.status == 401) 
        {
          this.auth.sesionExpirada( document.location.hash );
        }
        
      })
  }

  verMensajes( mensajes: any, leido: boolean )
  {        
    this.ultimoChat = mensajes.idChat;
    this.historialMensajes = mensajes;
    //console.log( this.historialMensajes );
    this.chatSelect = true;

    setTimeout(()=>{
      let objDiv: any = document.getElementById('scroll');
      objDiv.scrollTop = objDiv?.scrollHeight
    },0);

    if (leido && mensajes.cantidadnoLeido > 0 ) 
    {
      this.mensajeLeido( this.historialMensajes.idChat );
    }    
  }

  EnviarMensaje()
  {
    this._submitMensaje = true;
    //console.log( this.historialMensajes );
    this.mensajeForm.controls['Id_receptor'].setValue( this.historialMensajes.idEmisor > 0 ? this.historialMensajes.idEmisor : this.historialMensajes.idReceptor );
    
    if (this.mensajeForm.invalid) 
    {
      return;
    }   

    this.mensajeriaService.enviarMensaje( this.mensajeForm.value )
    .then( e => 
      {
        if (e.status == 200)
        {   
          //console.log(e);
          //this.chats();
          this.mensajeForm.reset();
        }

        if (e.status == 401)
        {   
          this.auth.sesionExpirada( document.location.hash );
        }
        
      });    
  }

  mensajeLeido( id: number )
  {
    let model = {
      Id: id
    };

    this.mensajeriaService.mensajeLeido( model )
    .then( e => 
      {
        if(e.status == 401)
        {
          this.auth.sesionExpirada( document.location.hash );
        }

      })
  }

}
