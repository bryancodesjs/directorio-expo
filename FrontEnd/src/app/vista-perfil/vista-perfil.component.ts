import { Component, OnInit, HostListener } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DashboardService } from '../services/dashboard/dashboard.service';
import { MensajeriaService } from '../services/mensajeria/mensajeria.service';

@Component({
  selector: 'app-vista-perfil',
  templateUrl: './vista-perfil.component.html',
  styleUrls: ['./vista-perfil.component.scss']
})
export class VistaPerfilComponent implements OnInit {

  obras: any = [];
  infoUser: any = '';
  error: boolean = false;
  currentUser: any = '';
  contactarModal: boolean = false;
  _submitMensaje: boolean = false;
  TiposSolicitud: any = [];
  
  //La siguiente funcion escucha el evento de scroll para reposicionar el card con la info del artista
  @HostListener("window:scroll", ['$event'])
  scrollRead(){
    //let scrollOffset = $event.srcElement.children[0].scrollTop;
    let number = scrollY; //offset vertical de la pantalla
    let screenWidth = screen.width; //ancho de pantalla
    let user = document.getElementById('userField'); //el card con la info del artista
    if (screenWidth >= 992 ) { //si el ancho de pantalla es mayor a 992px...
      if (number > 0) {
        number >  241 ? user?.setAttribute("style", "transform: translateY(100px); transition: ease 1s;") : user?.setAttribute("style", "transform: translateY(0px); transition: ease 1s;");
        //console.log(number);
      }
    }
  }

  mensajeForm = new FormGroup({
    Id_receptor: new FormControl('', Validators.required),
    asunto: new FormControl('', Validators.required),
    tipo_solicitud: new FormControl('', Validators.required),
    detalles: new FormControl('', Validators.required)  
  });

  get m() { return this.mensajeForm.controls; }  

  constructor( private mensajesService: MensajeriaService, private _route: ActivatedRoute, private dashService: DashboardService, private route: Router ) 
  {  
  }

  ngOnInit(): void 
  {
    this.getInfoUser( this._route.snapshot.paramMap.get('id') ); 
    this.getObras( this._route.snapshot.paramMap.get('id') );
  }

  Contactar()
  {    
    this.tiposSolicitud();
    this.contactarModal = true;    
    this.mensajeForm.controls['Id_receptor'].setValue( this._route.snapshot.paramMap.get('id') );    
  }

  async tiposSolicitud()
  {
    await this.mensajesService.tipoSolicitud()
    .then( e => 
      {
        if (e.status == 200) 
        {
          this.TiposSolicitud = e.message          
        }
        //console.log(e);
      });
  }

  EsconderModalMensajes()
  {
    this.contactarModal = false;
  }

  EnviarMensaje()
  {
    this._submitMensaje = true;
    
    if (this.mensajeForm.invalid) 
    {
      return;
    }

    this.mensajesService.enviarMensaje( this.mensajeForm.value )
    .then( e => 
      {
        if (e.status == 200)
        {
          this.contactarModal = false;
          this.route.navigate( ['/mensajeria', true]);
          //document.location.href = "mensajeria";    
          //console.log(e)   
        }
        
      });    
  }

  getObras(id: any)
  {
    this.dashService.getObrasUser( id )
    .then( e => { 
      //console.log(e)
      if ( e.status == 200 ) 
      {        
        this.obras.push(...e.message.obras);        
      }
      
    });
  }

  getInfoUser(id: any)
  {
    this.dashService.getInfoUser(id)
    .then( e => 
      { 
        const { status, message } = e;

        if (status == 0) 
        {
          this.error = true;
          return;
        }

        if (status == 200) 
        {
          this.infoUser = message;
          //console.log(this.infoUser);  
          this.error = false;
        }
    
      });
  }

  imagenError(event: any) 
  {    
    event.target.src = '../../assets/default-img.png';
  }

}
