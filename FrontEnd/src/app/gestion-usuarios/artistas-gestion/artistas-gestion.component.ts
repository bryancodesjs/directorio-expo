import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { GestionUsuariosService } from 'src/app/services/gestion_usuarios/gestion-usuarios.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-artistas-gestion',
  templateUrl: './artistas-gestion.component.html',
  styleUrls: ['./artistas-gestion.component.scss']
})
export class ArtistasGestionComponent implements OnInit {
  @Input() artistas: any[] = [];
  @Output() Actualizar = new EventEmitter<string>();

  constructor( private gestionService: GestionUsuariosService, private auth: AuthService ) { }

  ngOnInit(): void 
  {    
  }

  Deshabilitar( id:number )
  {
    let model = {
      Id: id
    }

    this.gestionService.Deshabilitar(model)
    .then( e => 
      {
        if (e.status == 200) 
        {
          //console.log(e);
          this.Actualizar.emit();
        }  
        if (e.status == 401) 
        {
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );
        }
        if (e.status == 203) 
        {  
          Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: 'Sin autorización...',
            showConfirmButton: false,
            timer: 2300
            });                  
        }
      })
  }

  Habilitar( id:number )
  {
    let model = {
      Id: id
    }

    this.gestionService.Habilitar(model)
    .then( e => 
      {
        if (e.status == 200) 
        {
          //console.log(e);
          this.Actualizar.emit();
        }  
        if (e.status == 401) 
        {
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );
        }
        if (e.status == 203) 
        {  
          Swal.fire({
            position: 'top-end',
            icon: 'error',
            title: 'Sin autorización...',
            showConfirmButton: false,
            timer: 2300
            });                  
        }
        
      })   
  }

}
