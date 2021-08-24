import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ActualizarObraPendiente } from '../../models';
import Swal from 'sweetalert2'
import { BackofficeService } from 'src/app/services/backoffice/backoffice.service';
import { HomeService } from 'src/app/services/home/home.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-publicaciones',
  templateUrl: './publicaciones.component.html',
  styleUrls: ['./publicaciones.component.scss']
})
export class PublicacionesComponent implements OnInit {
  @Output() Actualizar = new EventEmitter<string>();
  @Input() obrasPendientes: any = [];
  
  violaciones: any = [];
  public rechazar: number = 0;  
  submitted = false;

  rechazarForm = new FormGroup({
    Id: new FormControl(''),
    Accion: new FormControl(''),
    Id_Violacion: new FormControl('', Validators.required),
    Detalle: new FormControl('')  
  });

  get f() { return this.rechazarForm.controls; } 

  constructor( private backOfiService: BackofficeService, private route: Router, private homeService: HomeService, private auth: AuthService ) { }

  ngOnInit(): void
  {    
    //console.log(this.obrasPendientes)    
  } 

  rechazarMenu( id: any )
  { 
    //console.log(id)
    this.cargarViolaciones();
    (document.getElementById('data-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center d-none'; 
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center';
  }

  cancelarRechazo( id: number )
  {
    this.rechazarForm.controls['Id_Violacion'].setValue('');
    this.rechazarForm.controls['Detalle'].setValue('');
    (document.getElementById('data-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center'; 
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center d-none';
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

  imagenError(event: any) 
  {
    let ruta = '../../assets/default-img.png';
    event.target.src = ruta;
  }

  aprobarObra( id: any,  $event: any )
  {
    $event.preventDefault();   

    Swal.fire({
      title: '¿Estas seguro?',
      text: "¿Realmente quieres aprobar esta obra?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {        
        this.rechazarForm.controls['Id'].setValue(id);
        this.rechazarForm.controls['Accion'].setValue("Aceptar Publicacion");   

        this.backOfiService.actualizarObrasPendientes( this.rechazarForm.value )
        .then( e => 
          {        
            //console.log(e.message.status)
            if (e.status == 203) {
              Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Sin autorización...',
                showConfirmButton: false,
                timer: 2300
              });
            }

            if (e.status == 200) {
              Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Obra aprobada',
                showConfirmButton: false,
                timer: 1500
              });
  
              setTimeout( () => {
                //window.location.reload();
                this.Actualizar.emit();
              }, 1200);
            } 
            
            if (e.status == 401) {
              this.auth.logOut();
              this.auth.sesionExpirada( document.location.hash );
            }
          });
      }
    })   
 
  } 

  rechazarObra( id: any,  $event: any )
  {
    $event.preventDefault();    
    this.submitted = true; 

    if(!this.rechazarForm.valid)
    {
      return;
    }

    // SE CONFIRMA LA ACCION 
    Swal.fire({
      title: '¿Estas seguro?',
      text: "¿Realmente quieres rechazar esta obra?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) { 
        this.rechazarForm.controls['Id'].setValue(id);
        this.rechazarForm.controls['Accion'].setValue("Rechazar Publicacion");      

        //  SE LLAMA AL SERVICIO
        this.backOfiService.actualizarObrasPendientes( this.rechazarForm.value )
        .then( e => 
          {        
            if (e.status == 203) {
              Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Sin autorización...',
                showConfirmButton: false,
                timer: 2300
              });
            }

            if (e.status == 200) {
              Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Obra rechazada',
                showConfirmButton: false,
                timer: 1500
              });
  
              setTimeout( () => {
                //window.location.reload();
                this.Actualizar.emit();
              }, 1200);
            }
            
            if (e.status == 401) {
              this.auth.logOut();
              this.auth.sesionExpirada( document.location.hash );
            }
    
          });
      }
    });    
   
  }

}
