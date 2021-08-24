import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BackofficeService } from 'src/app/services/backoffice/backoffice.service';
import { HomeService } from 'src/app/services/home/home.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.scss']
})
export class TicketsComponent implements OnInit {
  @Output() Actualizar = new EventEmitter<string>();
  @Input() ticketPendientes: any[] = [];
  
  violaciones: any = [];
  submitted = false;
  rechazar: boolean = false;

  rechazarForm = new FormGroup({
    Id: new FormControl(''),
    Accion: new FormControl(''),
    Id_Violacion: new FormControl('', Validators.required),
    Detalle: new FormControl('')  
  });

  get f() { return this.rechazarForm.controls; } 

  constructor( private backOfficeService: BackofficeService, private homeService: HomeService, private auth: AuthService ) { }

  ngOnInit() 
  { }

  imagenError(event: any) 
  {
    let ruta = '../../assets/default-img.png';
    event.target.src = ruta;
  }

  aprobarTicket( id: any )
  {    
    this.submitted = true; 

    if(!this.rechazarForm.valid && this.rechazar == true)
    {
      return;
    }
    
    Swal.fire(
      {
      title: '¿Estas seguro?',
      text: "¿Realmente quieres aceptar esta solicitud?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
      })
    .then( (result) => 
    {
      if (result.isConfirmed) 
      {
        this.rechazarForm.controls['Id'].setValue(id);
        this.rechazarForm.controls['Accion'].setValue(true);

        this.backOfficeService.solicitudCambiosArtista( this.rechazarForm.value )
        .then( e => 
            {
              if (e.status == 200) 
              {  
                Swal.fire(
                  {
                  position: 'top-end',
                  icon: 'success',
                  title: 'Solicitud actualizada!',
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
                Swal.fire(
                  {
                  position: 'top-end',
                  icon: 'error',
                  title: 'Sin autorización...',
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
    });
  }

  rechazarTicket( id: any )
  {    
    this.submitted = true; 

    if(!this.rechazarForm.valid)
    {
      return;
    }

    Swal.fire(
      {
      title: '¿Estas seguro?',
      text: "¿Realmente quieres rechazar esta solicitud?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Si, hazlo!',
      cancelButtonText: 'Cancelar'
      })
    .then( (result) => 
    {
      if (result.isConfirmed) 
      {
        this.rechazarForm.controls['Id'].setValue(id);
        this.rechazarForm.controls['Accion'].setValue(false);

        this.backOfficeService.solicitudCambiosArtista( this.rechazarForm.value )
        .then( e => 
            {
              if (e.status == 200) 
              {  
                Swal.fire(
                  {
                  position: 'top-end',
                  icon: 'success',
                  title: 'Solicitud actualizada!',
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
                Swal.fire(
                  {
                  position: 'top-end',
                  icon: 'error',
                  title: 'Sin autorización...',
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

  rechazarMenu( id: any )
  { 
    this.rechazar = true;
    this.cargarViolaciones();
    (document.getElementById('data-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center d-none'; 
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center';
  }

  cancelarRechazo( id: number )
  {
    this.rechazar = false;
    this.rechazarForm.controls['Id_Violacion'].setValue('');
    this.rechazarForm.controls['Detalle'].setValue('');
    (document.getElementById('data-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center'; 
    (document.getElementById('rechazar-'+id) as HTMLElement).className = 'actions-wrap d-flex justify-content-center align-items-center d-none';
  }

}
