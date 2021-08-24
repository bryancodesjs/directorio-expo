import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { DashboardService } from '../services/dashboard/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  obras: any = [];
  visitas: number = 0;
  valoraciones: number = 0;

  error: boolean = false;  
  infoUser: any = '';
  motivoRechazo: any = ''; 
  
  perfilCargado: boolean = false;

  constructor( private auth: AuthService, private dashService: DashboardService ) { }  

  ngOnInit(): void 
  {
    this.getUser();       
  }

  imagenError(event: any) 
  {    
    event.target.src = '../../assets/no-image-home.png';
  }

  getUser()
  {
    this.getInfoUser();
    this.getObras();
  }

  getObras()
  {
    this.dashService.getObrasArtista()
    .then( e => {       
      if ( e.status == 200 ) 
      {        
        //console.log(e.message)
        this.obras.push(...e.message.obras);
        this.visitas = e.message.visitas != null ? e.message.visitas : 0
        this.valoraciones = e.message.valoraciones != null ? e.message.valoraciones : 0
      }  
      
      if ( e.status == 401 ) 
      {
        this.auth.logOut();
        this.auth.redireccionar( document.location.hash );
      }

    });
  }

  getInfoUser()
  {
    this.dashService.getInfoArtista()
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
          //console.log(message.userInfo)
          this.infoUser = message.userInfo;           
          this.motivoRechazo = message.rechazo;
          this.error = false;
          this.perfilCargado = true;
        }
        
        if(status == 401)
        {          
          this.auth.logOut();
          this.auth.sesionExpirada( document.location.hash );
        }        
      })
    .catch( e => console.log(e) )
  }


}
