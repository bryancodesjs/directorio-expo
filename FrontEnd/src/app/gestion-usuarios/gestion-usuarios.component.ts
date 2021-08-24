import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { GestionUsuariosService } from '../services/gestion_usuarios/gestion-usuarios.service';

@Component({
  selector: 'app-gestion-usuarios',
  templateUrl: './gestion-usuarios.component.html',
  styleUrls: ['./gestion-usuarios.component.scss']
})
export class GestionUsuariosComponent implements OnInit {
  ventPrivi: boolean = false;
  artistasButton: boolean = true;
  administrativoButton: boolean = false;

  artistas: [] = [];
  administradores: [] = [];
  artistaVentPrivileg: string = '';
  idArtistaVentPrivileg: number = 0;
  permisos: any[] = [];
  permisosRestantes: any[] = [];
  rol: string = "";
  superAdmin: boolean = false;

  constructor( private gestionService: GestionUsuariosService, private auth: AuthService ) { }

  ngOnInit(): void 
  {
    this.Check();
    this.Usuarios();
  }

  Permisos( value: any, $event: any )
  {    
    const acceso = $event.target.checked;    
    if (acceso) 
    {
      this.gestionService.agregarPermisos( value )
      .then( e => 
        {
          //console.log(e);
          if (e.status == 200) 
          {
            //console.log("Listo");
            this.ActualizarPrivilegios();
          }
          if (e.status == 404) 
          {
            console.log("Hubo un error");
          }
          if (e.status == 401) {
            this.auth.logOut();
            this.auth.sesionExpirada( document.location.hash );
          }
        });
    }
    else
    {
      this.gestionService.quitarPermisos( value )
      .then( e => 
        {
          //console.log(e);
          if (e.status == 200) 
          {
            //console.log("Listo");
            this.ActualizarPrivilegios();
          }
          if (e.status == 404) 
          {
            console.log("Hubo un error");
          }
          if (e.status == 401) {
            this.auth.logOut();
            this.auth.sesionExpirada( document.location.hash );
          }
        });
    }
  }

  Usuarios()
  {
    this.gestionService.Usuarios()
    .then( e => 
      {        
        this.artistas = e.message.artistas;
        this.administradores = e.message.administradores;
        //console.log(this.administradores);
      })
  }

  Check()
  {
    this.gestionService.Check()
    .then( e =>
      {
        if (e.status == 200) 
        {
          this.superAdmin = e.message 
          //console.log(e) 
        }
      })
  }

  Buttons( name: string )
  {
    if (name == 'administrativo') 
    {
      this.administrativoButton = true;
      this.artistasButton = false;
    }
    if (name == 'artista') 
    {
      this.administrativoButton = false;
      this.artistasButton = true;
    }
  }

  CancelarVentPrivi()
  {
    this.ventPrivi = false;
  }

  Privilegio( value: any )
  {
    //console.log(value)
    this.artistaVentPrivileg = value.artista;
    this.idArtistaVentPrivileg = value.id;
    this.ventPrivi = true;
    let model = {
      'Id': value.id
    }
    this.gestionService.Permisos(model)
    .then( e =>
      {        
        //console.log(e);
        this.permisos = e.message.permisosUsuario;
        this.permisosRestantes = e.message.permisosRestantes;
        this.rol = e.message.rol;
        // console.log(this.permisos);
      });
  } 
  
  Actualizar()
  {
    this.Usuarios();
  }

  ActualizarPrivilegios()
  {
    let model = {
      artista: this.artistaVentPrivileg,
      id: this.idArtistaVentPrivileg
    }
    this.Privilegio(model);
  }

}
