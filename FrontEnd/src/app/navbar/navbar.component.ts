import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscriber } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
import { MensajeriaService } from '../services/mensajeria/mensajeria.service';
import Swal from 'sweetalert2';
import { DashboardService } from '../services/dashboard/dashboard.service';
import { NgxImageCompressService } from 'ngx-image-compress';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  logueado: any = null;
  admin: boolean = false;
  errorConexion: boolean = false;
  nombreUsuario: string = '';
  permisoArtista_Empresa: boolean = false;

  showModal: boolean = false;
  imgBase64: any = null;
  submitted = false;

  publicacionForm = new FormGroup({   
    nombreObra: new FormControl('', Validators.required),
    descripcionObra: new FormControl('', Validators.required),
    ubicacion: new FormControl('', Validators.required),
    imgObra: new FormControl('', Validators.required)    
  });

  get f() { return this.publicacionForm.controls; }

  constructor( private auth: AuthService, private mensajeria: MensajeriaService, private dashService: DashboardService, private imageCompress: NgxImageCompressService ) {}

  ngOnInit()
  {    
    this._Acceso();
  }

  _Acceso() {
    this.auth.Acceso().then( (res) => 
    {      
      // SI LA RESPUESTA FUE EXITOSA
      if (res.status == 200) 
      {
        // SE OBTIENE EL NOMBRE DEL USUARIO
        this.nombreUsuario = res.usuario; 
        // SE VERIFICA QUE EL USUARIO SEA ARTISTA O EMPRESA 
        this.permisoArtista_Empresa = res.permisoArtis_Empre;
        //console.log(res.permisoArtis_Empre);
        // SI NO ES ADMINISTRADOR
        if (!res.message) 
        {
          this.logueado = true;
          this.admin = false;
          this.errorConexion = false;
        }
        // SI ES ADMINISTRADOR
        if (res.message) 
        {
          this.logueado = true;
          this.admin = true;
          this.errorConexion = false;
        }
      }
      // SI NO HA INICIADO SESION
      if (res.status == 401) 
      {
        this.logueado = false;
        this.admin = false;
        this.errorConexion = false;
        this.auth.logOut();
        //this.auth.redireccionar( document.location.hash );
      }
      // SI OCURRE UN ERROR DE CONEXION
      if (res.status == 0) 
      {
        this.errorConexion = true;
      }

    });
  }

  LogOut() 
  {
    this.mensajeria.desConectado();  
    this.logueado = false;
    this.admin = false;
    this.auth.logOut();      
    //document.location.href = '/#';
  }

  mostrarBusqueda(): boolean
  {
    if( document.location.hash == "#/" )
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  show()
  {    
    this.showModal = true;
    //console.log("e")
  }

  hide()
  {
    this.showModal = false;    
    this.publicacionForm.reset();
  }

  ComprimirPerfil() 
  {  
    this.imageCompress.uploadFile()
    .then( ({image, orientation}) => 
    {
      this.imageCompress.compressFile(image, orientation, 50, 50)
      .then( result => 
        {
          this.imgBase64 = result;
        }
      );      
    });    
  }  

  submitPublicacion()
  { 
    this.submitted = true;    
    this.publicacionForm.controls['imgObra'].setValue( this.imgBase64 );
    //this.publicacionForm.controls['_Id'].setValue( this.infoUser.id );
    
    if (this.publicacionForm.invalid) 
    {
      //this.submitted = false;
      return;
    }
    
    Swal.fire({
      title: '¿Estas seguro?',
      text: "¿Realmente quieres publicar esta obra?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {        

        this.dashService.nuevaPublicacion( this.publicacionForm.value )
        .then( e => 
          {
            if (e.status == 200) 
            {
              this.showModal = false;
              this.publicacionForm.reset();
             
              Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Publicado correctamente!',
                showConfirmButton: false,
                timer: 1500
              });
              // SE LLAMA A ESTE METODO PARA OBTENER LAS NUEVAS OBRAS
              //this.getObras();    
            }
            if (e.status == 500) 
            {
              this.showModal = false;
              this.publicacionForm.reset();
             
              Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Algo anda mal, vuelva a intentarlo mas tarde...',
                showConfirmButton: false,
                timer: 3500
              });    
            }
            //console.log(e);
          });
      
      }
    })
    
  }

}
