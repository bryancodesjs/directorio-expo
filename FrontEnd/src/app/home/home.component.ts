import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ContadorValoracionesService } from '../services/contador_valoraciones/contador-valoraciones.service';
import { DenunciasService } from '../services/home/denuncias.service';
import { HomeService } from '../services/home/home.service';
import { VisitasService } from '../services/home/visitas.service';
import { MensajeriaService } from '../services/mensajeria/mensajeria.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  imgLoading: string = '../../assets/img/loading.gif';

  ordenar: string = '';
  iniciarSesion: boolean = false;
  valoracionCorrecta: boolean = false;
  valoracionEnviada: boolean = false;
  denunciar: boolean = false;
  masdetalles: boolean = false;
  denunciaExistente: boolean = false;
  denunciaEnviada: boolean = false;

  galeria: any = [];
  violaciones: any = [];
  TiposSolicitud: any = [];
  submitted = false;
  showModal: boolean = false;
  loading: boolean = true;
  error: boolean = false;

  _contactarModal: boolean = false;
  _datosContactoModal: any = [];
  _submitMensaje: boolean = false;

  obraModal = {
    id_perfil: 0,
    id: 0,
    artista: '',
    imgObra: '',
    nombreObra: '',
    fechaRegistro: '',
    ubicacion: '',
    valoraciones: 0,
    visitas: 0,
    autenticado: '',
    valorado: ''    
  };

  denunciasForm = new FormGroup({
    Id_Obra: new FormControl(''),
    Id_Artista: new FormControl(''),
    Id_Violacion: new FormControl('', Validators.required),
    Detalle: new FormControl('')  
  });

  mensajeForm = new FormGroup({
    Id_receptor: new FormControl('', Validators.required),
    asunto: new FormControl('', Validators.required),
    tipo_solicitud: new FormControl('', Validators.required),
    detalles: new FormControl('', Validators.required)  
  });

  get f() { return this.denunciasForm.controls; }  
  get m() { return this.mensajeForm.controls; }  

  constructor( private router: Router, private homeService: HomeService, private visitasService: VisitasService, private valoracionService: ContadorValoracionesService, private denunciasService: DenunciasService, private mensajesService: MensajeriaService ) {  }

  ngOnInit(): void 
  {    
    this.cargarGaleria();    
  }   

  Ordenar( _ordenar: any )
  {
    this.ordenar = _ordenar;    
    this.cargarGaleria();
  }

  cargarGaleria()
  {
    this.homeService.obtenerGaleria( this.ordenar )
    .then( (e: any) => 
      {
        if (e.status == 0) 
        {
          this.error = true; 
          return;
        }

        const { message } = e;
        //console.log(message)
        this.galeria = message; 
        this.loading = false;
        this.error = false; 
      });
  }

  cargarViolaciones()
  {
    this.homeService.obtenerViolaciones()
    .then( (e: any) => 
      {
        if (e.status == 0) 
        {          
          return;
        }        
        //console.log(message)
        this.violaciones = e.message;         
      });
  }

  async valorar( id: number )
  {
    //console.log(id);
    if( await this.revisarValoracion( id ) )
    {
      this.valoracionService.nuevaValoracion( id )
      .then( e => 
        {
          if(e.status == 200)
          {
            this.valoracionEnviada = true;
          }
        });
    }    
  }

  async denunciarObra( idObra: number )
  {    
    this.submitted = true;
    this.denunciasForm.controls['Id_Obra'].setValue( idObra );

    if (this.denunciasForm.invalid) 
    {
      //this.submitted = false;
      return;
    }         

    await this.denunciasService.denunciarObra( this.denunciasForm.value )
    .then( e => 
      {
        if( e.status == 200 )
        {         
          this.cancelarDenuncia();
          this.denunciaEnviada = true;
          setTimeout( () => { this.denunciaEnviada = false; }, 4100)
        } 
        if( e.status == 403 )
        {          
          this.submitted = false;
          this.denunciaExistente = true;
          this.cancelarDenuncia();
          setTimeout( () => { this.denunciaExistente = false; }, 4100)
        }     
        
      });
  }

  cancelarDenuncia()
  {
    this.denunciar = false;
    this.masdetalles = false;
    this.denunciasForm.controls['Id_Violacion'].setValue( '' );
    this.denunciasForm.controls['Detalle'].setValue( '' );
  }

  async revisarValoracion( id: number )
  {
    return await this.valoracionService.revisarValoracion( id )
    .then( e => 
      {
        if( e.status == 200 )
        {
          return true;
        }        
        if( e.status == 401 )
        {
          this.iniciarSesion = true;
          return false
        }
        else 
        {
          this.valoracionCorrecta = true;
          return false;
        }
      });
  }
  
  show( id: any )
  {
    //console.log(id)
    this.showModal = true;   
    this.obraModal.id_perfil = id. id;
    this.obraModal.id = id.idObra;
    this.obraModal.artista = id.artista;
    this.obraModal.imgObra = id.imgObra;
    this.obraModal.nombreObra = id.nombreObra;
    this.obraModal.fechaRegistro = id.fechaRegistro;
    this.obraModal.valoraciones = id.valoraciones;
    this.obraModal.visitas = id.visitas;
    this.obraModal.autenticado = id.autenticado;
    this.obraModal.valorado = id.valorado;

    this.visitasService.VerificarVisita( id.idObra );  
    //console.log(id)
  }

  Denunciar()
  {
    this.denunciar = true;
    this.cargarViolaciones();
  } 

  masDetalles()
  {
    this.masdetalles = true;
  }

  ocultarModal()
  {
    this.showModal = false;    
    this.iniciarSesion = false;
    this.valoracionCorrecta = false;
    this.valoracionEnviada = false;
    this.cancelarDenuncia();
  }

  imagenError(event: any) 
  {    
    event.target.src = '../../assets/default-img.png';
  }

  Contactar( artista: any )
  {
    this.tiposSolicitud();
    this._contactarModal = true;
    this._datosContactoModal = artista;
    this.mensajeForm.controls['Id_receptor'].setValue( artista.id_perfil );
    //console.log(artista);
  }

  EsconderModalMensajes()
  {
    this._contactarModal = false;
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
          this.router.navigate( ['/mensajeria', true]);
          //document.location.href = "mensajeria";
          //console.log(e);
        }
        
      });
    //console.log(this.mensajeForm.value);
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

}
