import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate 
{
  constructor( private auth: AuthService ) {}

  canActivate(): any
  {
    return this.auth.Acceso()
    .then( res =>
      {
        //console.log(res)
        if ( res.status == 200 ) 
        {
            if ( !res.message && res.permisoArtis_Empre ) 
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
          this.auth.logOut();
          return this.auth.redireccionar( document.location.hash );          
        }
      });
  }
  
}
