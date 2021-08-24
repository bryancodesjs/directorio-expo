
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ConfiguracionService } from 'src/app/services/configuracion/configuracion.service';
import { DashboardService } from 'src/app/services/dashboard/dashboard.service';

import {NgxImageCompressService} from 'ngx-image-compress';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-configuracion',
  templateUrl: './configuracion.component.html',
  styleUrls: ['./configuracion.component.scss']
})
export class ConfiguracionComponent implements OnInit {  

  solicitudesRealizadas: any[] = [];
  cargando: boolean = false;
  showModal: boolean = false;
  misDatos: boolean = true;
  servicios: boolean = false;  
  personalizacion: boolean = false;
  contacto: boolean = false;
  seguridad: boolean = false;
  solicitudes: boolean = false;
  categoriaServicios: any = [];
  tipoServicios: any = [];

  submit: boolean = false;
  submitCategoria: boolean = false;
  submitTipoServicio: boolean = false;
  errorClave: boolean = false;

  nuevaFotoPerfil: any = null;
  nuevaFotoBanner: any = null;

  misDatosForm = new FormGroup({    
    nombre: new FormControl('', Validators.required),
    apellido: new FormControl('', Validators.required),    
    nacionalidad: new FormControl('', Validators.required),    
    lugarNacimiento: new FormControl('', Validators.required),    
    rangoEdad: new FormControl('', Validators.required),  
    genero: new FormControl('', Validators.required),  
    cedula: new FormControl('', Validators.required),    
    direccion_residencia: new FormControl('', Validators.required),        
    provincia: new FormControl('', Validators.required),  
    municipio: new FormControl('', Validators.required),      
  });

  categoriaServiciosForm = new FormGroup({    
    id: new FormControl('', Validators.required),
    nombre: new FormControl('', Validators.required)    
  });

  tipoServiciosForm = new FormGroup({    
    id: new FormControl('', Validators.required),
    nombre: new FormControl('', Validators.required)    
  });

  contactoForm = new FormGroup({    
    facebook: new FormControl(''),
    youtube: new FormControl(''),
    instagram: new FormControl(''),
    enlace_Paginaweb: new FormControl(''),
    twitter: new FormControl(''),
    linkedin: new FormControl(''),
    email: new FormControl('', Validators.required),
    telefono: new FormControl('', Validators.required),
    telefono_celular: new FormControl(null, Validators.required),
    telefono_fijo: new FormControl('', Validators.required)
  });

  nuevaClaveForm = new FormGroup({    
    actual: new FormControl('', Validators.required),
    nueva: new FormControl('', Validators.required),
    confirmar_nueva: new FormControl('', Validators.required)        
  });

  FotosForm = new FormGroup({    
    img_perfil: new FormControl(''),
    img_banner: new FormControl('')    
  });

  constructor( private dashService: DashboardService, private auth: AuthService, private configService: ConfiguracionService, private imageCompress: NgxImageCompressService ) 
  {         
  }

  ngOnInit(): void 
  {
    this.datosArtista();    
  }

  mostrarModal()
  {    
    this.showModal = true;    
  }

  esconderModal()
  {
    this.showModal = false;
  }

  imagenError(event: any) 
  {
    let ruta = '../../assets/default-img.png';
    event.target.src = ruta;
  }

  // CONTROLES PARA VALIDAR LOS FORMULARIOS Y MOSTRAR LOS ERRORES
  get categoriaServiciosControls() { return this.categoriaServiciosForm.controls; }
  get tipoServiciosControls() { return this.tipoServiciosForm.controls; }
  get misDatosControls() { return this.misDatosForm.controls; }
  get seguridadControls() { return this.nuevaClaveForm.controls; }  
  get contactoControls() { return this.contactoForm.controls; }  

  actualizarFotos()
  { 
    this.submit = true;

    if ( this.FotosForm.value.img_perfil == null && this.FotosForm.value.img_banner == null) 
    {  
      Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'No ha seleccionado nada nuevo'
      })     
      return;
    }     
    this.configService.ActualizarPerfil( this.FotosForm.value )
    .then
    ( e => 
      {
        console.log(e);
        const { status, message } = e;
        if (status == 200) 
        {
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Solicitud de cambios enviada!',
            showConfirmButton: false,
            timer: 2700
          });

          setTimeout(() => {
            document.location.reload();
          }, 1800);

          return;
        }

        if (status == 401) 
        {
           console.log( message );
           this.auth.logOut();
           this.auth.sesionExpirada( document.location.hash );
           return;
        }

      }
    );
  }

  nuevaCategoria()
  {
    this.submitCategoria = true;
    console.log( this.categoriaServiciosForm.value );
  }

  nuevoServicio()
  {
    this.submitTipoServicio = true;
    console.log( this.tipoServiciosForm.value );
  }

  ComprimirPerfil() 
  {  
    this.imageCompress.uploadFile()
    .then( ({image, orientation}) => 
    {        
      this.imageCompress.compressFile(image, orientation, 50, 50)
      .then( result => 
        {
          this.nuevaFotoPerfil = result;
          this.FotosForm.value.img_perfil = result;         
        }
      );      
    });    
  }
  
  ComprimirBanner() 
  {  
    this.imageCompress.uploadFile()
    .then( ({image, orientation}) => 
    {   
      this.imageCompress.compressFile(image, orientation, 50, 50)
      .then( result => 
        {
          this.nuevaFotoBanner = result;
          this.FotosForm.value.img_banner = result;          
        }
      );      
    });    
  }  
 
  actualizarMisDatos()
  {
    this.submit = true;

    if ( this.misDatosForm.invalid ) 
    { 
      return;
    }     

    console.log(this.misDatosForm.value);

    // this.configService.ActualizarPerfil( this.misDatosForm.value )
    // .then
    // ( e => 
    //   {
    //     const { status, message } = e;
    //     if (status == 200) 
    //     {
    //       Swal.fire({
    //         position: 'top-end',
    //         icon: 'success',
    //         title: 'Solicitud de cambios enviada!',
    //         showConfirmButton: false,
    //         timer: 2700
    //       });

    //       setTimeout(() => {
    //         document.location.reload();
    //       }, 2800);

    //       return;
    //     }

    //     if (status == 401) 
    //     {
    //        console.log( message );
    //        this.auth.logOut();
    //        this.auth.sesionExpirada( document.location.hash );
    //        return;
    //     }

    //   }
    // );
  }

  actualizarContacto()
  {
    this.submit = true;

    if ( this.contactoForm.invalid ) 
    { 
      return;
    }     

    this.configService.ActualizarPerfil( this.contactoForm.value )
    .then
    ( e => 
      {
        const { status, message } = e;
        if (status == 200) 
        {
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Solicitud de cambios enviada!',
            showConfirmButton: false,
            timer: 2700
          });

          setTimeout(() => {
            document.location.reload();
          }, 2800);

          return;
        }

        if (status == 401) 
        {
           console.log( message );
           this.auth.logOut();
           this.auth.sesionExpirada( document.location.hash );
           return;
        }
        
      }
    );
  }

  submitSeguridad()
  {  
    this.submit = true;
    this.errorClave = false;
    //console.log(this.nuevaClaveForm.value)

    if ( this.nuevaClaveForm.invalid ) 
    {      
      return;
    }

    if (this.nuevaClaveForm.value.nueva != this.nuevaClaveForm.value.confirmar_nueva) 
    {
      this.errorClave = true;
      return;
    }

    this.configService.cambiarClave( this.nuevaClaveForm.value )
    .then( e => 
      {
        //console.log(e)
        if (e.status == 200) 
        {
          Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: 'Cambios realizados!',
            showConfirmButton: false,
            timer: 1500
          });

          setTimeout(() => {
            document.location.reload();
          }, 1600);
        }

        if (e.status == 404) 
        {
          Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: e.message,
            showConfirmButton: false,
            timer: 2300
          });
        }

        if (e.status == 401) 
        {
           this.auth.logOut();
           this.auth.sesionExpirada( document.location.hash );
        }
        
      });    
  }

  datosArtista()
  {
    this.cargando = true;
    this.dashService.getInfoArtista()
    .then( e => 
      { 
        const { status, message } = e;
        if (status == 200)
        { 
          console.log(message)
          this.categoriaServicios = message.categoriaServicios;
          this.tipoServicios = message.tipoServicios;          
          this.solicitudesRealizadas.push( ...message.solicitudes );
               
          const { 
           provincia, municipio, nombre, apellido, nacionalidad, lugar_nacimiento, rango_edad, genero, cedula_identidad, direccion_residencial, descripcionGeneral, telefono, telefono_celular, telefono_fijo, facebook, instagram, youtbe, email, imgBanner, img_perfil, enlace_Paginaweb, twitter, linkedin 
          } = message.userInfo;
          
          // MIS DATOS          
          this.misDatosForm.controls['nombre'].setValue( nombre );
          this.misDatosForm.controls['apellido'].setValue( apellido );   
          this.misDatosForm.controls['nacionalidad'].setValue( nacionalidad );
          this.misDatosForm.controls['lugarNacimiento'].setValue( lugar_nacimiento );      
          this.misDatosForm.controls['rangoEdad'].setValue( rango_edad );
          this.misDatosForm.controls['genero'].setValue( genero );
          this.misDatosForm.controls['cedula'].setValue( cedula_identidad );
          this.misDatosForm.controls['direccion_residencia'].setValue( direccion_residencial );  
          this.misDatosForm.controls['provincia'].setValue( provincia );   
          this.misDatosForm.controls['municipio'].setValue( municipio );    
         
          // CONTACTO  
          this.contactoForm.controls['telefono'].setValue( telefono );
          this.contactoForm.controls['facebook'].setValue( facebook );
          this.contactoForm.controls['youtube'].setValue( youtbe );
          this.contactoForm.controls['instagram'].setValue( instagram );
          this.contactoForm.controls['email'].setValue( email );
          this.contactoForm.controls['enlace_Paginaweb'].setValue( enlace_Paginaweb );
          this.contactoForm.controls['twitter'].setValue( twitter );
          this.contactoForm.controls['linkedin'].setValue( linkedin );
          this.contactoForm.controls['telefono_celular'].setValue( telefono_celular );
          this.contactoForm.controls['telefono_fijo'].setValue( telefono_fijo );

          // FOTOS           
          this.FotosForm.controls['img_perfil'].setValue( img_perfil );
          this.FotosForm.controls['img_banner'].setValue( imgBanner ); 

          this.cargando = false;
        }

        if (status == 401) 
        {
           this.auth.logOut();
           this.auth.sesionExpirada( document.location.hash );
        }
    
      });
  }

  select( opcion: string )
  {
    switch (opcion) {

      case 'misdatos':
        this.misDatos = true;
        this.personalizacion = false;
        this.contacto = false;
        this.seguridad = false;
        this.solicitudes = false;
        this.servicios = false;
        break;
      case 'servicios':
        this.servicios = true;
        this.misDatos = false;
        this.personalizacion = false;
        this.contacto = false;
        this.seguridad = false;
        this.solicitudes = false;
        break;

      case 'personalizacion':
        this.personalizacion = true;
        this.misDatos = false;
        this.contacto = false;
        this.seguridad = false;
        this.solicitudes = false;
        this.servicios = false;
        break;

      case 'contacto': 
        this.contacto = true;
        this.misDatos = false;
        this.personalizacion = false;
        this.seguridad = false;
        this.solicitudes = false;
        this.servicios = false;
        break;
      case 'seguridad':
        this.seguridad = true;
        this.misDatos = false;
        this.personalizacion = false;
        this.contacto = false;
        this.solicitudes = false;
        this.servicios = false;
        break;

      case 'solicitudes':
        this.solicitudes = true;
        this.seguridad = false;
        this.misDatos = false;
        this.personalizacion = false;
        this.contacto = false;    
        this.servicios = false;    
        break;

      default:
        this.misDatos = false;
        this.personalizacion = false;
        this.contacto = false;
        this.seguridad = false;
        this.solicitudes = false;
        this.servicios = false;
        break;
    }
  }

}