import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute  } from '@angular/router';

import { AuthService } from 'src/app/services/auth/auth.service';
import { MensajeriaService } from 'src/app/services/mensajeria/mensajeria.service';
import { LoginService } from './../../services/login/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {

  returnUrl: string = '';
  submitted = false;
  error = false;
  messageError = '';

  // GRUPO DE ELEMENTOS PARA FORMULARIO DE LOGIN
  loginForm = new FormGroup({
    email: new FormControl('', Validators.required),
    clave: new FormControl('', Validators.required)
  });

  constructor( private loginService: LoginService, private auth: AuthService, private route: Router, private router: ActivatedRoute, private mensajeria: MensajeriaService ) { }

  async ngOnInit(): Promise<void> 
  {

    this.router.queryParams
    //.filter( params => params.order )
    .subscribe( params => 
      {        //console.log(params);
        this.returnUrl = params?.returnUrl;
        //console.log(params);
      }
    );

    if( await this.auth.authorized() )
    {
      this.route.navigate(['/']);
    }
  }

  get f() { return this.loginForm.controls; }

  submit()
  {
    this.submitted = true;    

    if (this.loginForm.invalid) {
      return;
    }
   
    this.loginService.LoginPost(this.loginForm.value)
    .then( res => {      
      if(res.status == 200)
      {      
        this.mensajeria.Conectado();
        //document.location.href = res.redirect;
        if (this.returnUrl != undefined) 
        {
          this.route.navigate([ this.returnUrl ]);
        }
        else
        {
          this.route.navigate([ res.redirect ]);
        }
        return;
      }
      else
      {
        this.submitted = false;    
        this.error = true;
        this.messageError = res.message != null ? res.message : 'Algo no anda bien, vuelva mas tarde...' ;
        return;
      }
  
    });
    
  }


}
