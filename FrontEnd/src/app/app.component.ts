import { Component } from '@angular/core';
import { Router } from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  
  title = 'directorio-creativo';
  private current: string = '';

  constructor( private router: Router ) 
  {     
    this.router.events.subscribe( () => 
    {
      this.current = this.router.url;       
    });
  }
  
  ocultarNavbar()
  {
    //this.current.
    // setTimeout(() => {
    //   console.log(this.current.indexOf('/login'))
    // }, 3000);
    const ubicacionActual = document.location.hash;    

    if ( ubicacionActual.indexOf('/login') !== -1 || ubicacionActual.indexOf('/registro') !== -1 ) 
    {      
      return false;  
    }    
    else
    {
      return true;
    }
  }
}
