import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class MensajeriaGuard implements CanActivate {

  constructor( private auth: AuthService ) {}

  canActivate(): any
  {
    return this.auth.Acceso()
    .then( res =>
      {
        //console.log(res)
        if ( res.status == 200 ) 
        {            
          return true;           
        }
        else
        {
          this.auth.logOut();
          return this.auth.redireccionar( document.location.hash );          
        }
      });
  }
  
}
