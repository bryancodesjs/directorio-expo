import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { BackofficeService } from 'src/app/services/backoffice/backoffice.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate 
{ 
  constructor( private router: Router, private auth: AuthService ) {}

 canActivate(): Promise<any>
  {   
    return this.auth.Acceso()
    .then( res =>
      {        
        if ( res.status == 200 ) 
        {
            if ( res.message ) 
            { 
              return true;
            }
            else
            {
              return false;              
            }
        }
        else
        {
          return this.auth.redireccionar( document.location.hash );
        }
      }); 
  }

 
  
}
