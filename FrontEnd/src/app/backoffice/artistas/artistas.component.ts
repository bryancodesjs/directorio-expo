import { Component, Input, OnInit, ElementRef, Output, EventEmitter  } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BackofficeService } from 'src/app/services/backoffice/backoffice.service';
import Swal from 'sweetalert2';
import { HomeService } from 'src/app/services/home/home.service';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-artistas',
  templateUrl: './artistas.component.html',
  styleUrls: ['./artistas.component.scss']
})
export class ArtistasComponent implements OnInit {
  @Output() Actualizar = new EventEmitter<string>();
  @Input() _artistasPendientes: any[] = [];  
  violaciones: any = [];
  submitted = false;

  rechazarForm = new FormGroup({
    Id_Usuario: new FormControl(''),
    Id_Violacion: new FormControl('', Validators.required),
    Detalle: new FormControl('')  
  });

  get f() { return this.rechazarForm.controls; }  

  constructor( private backService: BackofficeService, private homeService: HomeService, private el: ElementRef, private auth: AuthService ) { }

  ngOnInit()
  { } 

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

  aprobarArtista( id: any )
  {
    let model: any = 
    {
      Id: id
    }

    Swal.fire({
      title: '¿Estas seguro?',
      text: "¿Realmente quieres aceptar este artista?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) { 
        
        this.backService.aprobarArtista( model )
        .then( e => 
          {
            if (e.status == 200) 
            {  
              Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Artista aceptado...',
                showConfirmButton: false,
                timer: 1500
              });

              setTimeout( () => {
                //window.location.reload();
                this.Actualizar.emit();
              }, 1200);
            }

            if (e.status == 203) 
            {  
              Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Sin autorización...',
                showConfirmButton: false,
                timer: 1500
              });              
            }

            if (e.status == 401) {
              this.auth.logOut();
              document.location.href = "/";
            }

          });
        
      }
    }) 

  }  
  
  rechazarMenu( id: any )
  { 
    this.cargarViolaciones();
    (document.getElementById('data-'+id) as HTMLElement).className = 'd-none';
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'd-display';
  }

  cancelarRechazo( id: number )
  {
    this.rechazarForm.controls['Id_Violacion'].setValue('');
    this.rechazarForm.controls['Detalle'].setValue('');
    (document.getElementById('data-'+id) as HTMLElement).className = 'd-display';
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'd-none';
  }

  rechazarArtista( id: number )
  {  
    this.submitted = true;
    this.rechazarForm.controls['Id_Usuario'].setValue(id);
    if (this.rechazarForm.invalid) 
    {      
      return;
    } 
    
    Swal.fire({
      title: '¿Estas seguro?',
      text: "¿Realmente quieres rechazar este artista?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) { 
        
        this.backService.rechazarArtista( this.rechazarForm.value )
        .then( e => 
          {
            if (e.status == 200) 
            {
              Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Artista rechazado...',
                showConfirmButton: false,
                timer: 1500
              });

              setTimeout( () => {
                //window.location.reload();
                this.Actualizar.emit();
              }, 1200);
            }
            if (e.status == 203) 
            {  
              Swal.fire({
                position: 'top-end',
                icon: 'error',
                title: 'Sin autorización...',
                showConfirmButton: false,
                timer: 1500
              });              
            }
            
            if (e.status == 401) {
              this.auth.logOut();
              this.auth.sesionExpirada( document.location.hash );
            }
            
          });
        
      }
    }) 
  }


}
