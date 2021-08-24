import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { LoginService } from 'src/app/services/login/login.service';
import Inputmask from "inputmask";

@Component({
  selector: 'app-registro',
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.scss']
})
export class RegistroComponent implements OnInit {

  tipoRegistro: string = '';
  menu: boolean = true;
  usuario: boolean = false;
  profesional: boolean = false;
  empresa: boolean = false;
  continuar: boolean = false;

  cargando: boolean = false;
  submitted = false;
  claveError = false;
  error = false;
  messageError = null;

  registroForm = new FormGroup({
    tipo_registro: new FormControl('', Validators.required),
    nombre: new FormControl('', Validators.required),
    apellido: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),      
    clave: new FormControl('', Validators.required),
    confirmar_clave: new FormControl('', Validators.required),
    leido: new FormControl('', Validators.required),
  });

  registroFormEmpresa = new FormGroup({
    tipo_registro: new FormControl('', Validators.required),
    nombre: new FormControl('', Validators.required),
    rnc: new FormControl('', Validators.required),    
    email: new FormControl('', Validators.required),
    telefono: new FormControl('', Validators.required),   
    clave: new FormControl('', Validators.required),
    confirmar_clave: new FormControl('', Validators.required),
  });

  get f() { return this.registroForm.controls; }
  get e() { return this.registroFormEmpresa.controls; }

  constructor( private loginService: LoginService, private route: Router, private auth: AuthService ) { }

  ngOnInit(): void 
  {  
    if(!this.auth.authorized())
    {
      this.route.navigate(['/']);
    }
  }

  submit()
  {
    this.cargando = true;
    this.submitted = true;
    this.claveError = false;  

    if (this.registroForm.invalid) 
    {
      console.log("validacion")      
      this.cargando = false;
      this.submitted = false;      
      return;
    }

    if (this.registroForm.value.clave !== this.registroForm.value.confirmar_clave) 
    {
      console.log("clavess")
      this.claveError = true;        
      return;
    }

    this.loginService.registroPost( this.registroForm.value )
    .then( e => 
      { 
        if(e.status == 200)
        {          
          this.error = false;
          setTimeout(() => {
            this.submitted = false;
            this.route.navigate([`${e.redirect}`]);
          }, 3300);  
          return;        
        } 
        if(e.status == 400)
        {
          this.cargando = false;
          this.submitted = false;    
          this.error = true;
          this.messageError = e.message;
          //console.log(e);
        }
        else
        {
          this.cargando = false;
          this.submitted = false;    
          this.error = true;
          this.messageError = e.message;
        }
      });
    
  }

  submitEmpresa()
  {    
    this.cargando = true;
    this.submitted = true;
    this.claveError = false;  

    if (this.registroFormEmpresa.invalid) 
    {
      this.cargando = false;
      this.submitted = false;     
      return;
    }

    if (this.registroFormEmpresa.value.clave !== this.registroFormEmpresa.value.confirmar_clave) 
    {
      this.claveError = true;        
      return;
    } 

    this.loginService.registroPost( this.registroFormEmpresa.value )
    .then( e => 
      { 
        if(e.status == 200)
        {          
          this.error = false;
          setTimeout(() => {
            this.submitted = false;
            this.route.navigate([`${e.redirect}`]);
          }, 3300);  
          return;        
        } 
        if(e.status == 400)
        {
          this.cargando = false;
          this.submitted = false;    
          this.error = true;
          this.messageError = e.message;
          //console.log(e);
        }
        else
        {
          this.cargando = false;
          this.submitted = false;    
          this.error = true;
          this.messageError = e.message;
        }
      });
  }

  avanzar( value: boolean )
  {
    this.continuar = value;

    this.registroForm.reset();
    this.registroFormEmpresa.reset();
    this.submitted = false;

    if(this.empresa)
    {
      this.registroFormEmpresa.controls['tipo_registro'].setValue(this.tipoRegistro);      
      setTimeout(() => {
        let selector: any = document.getElementById("phone");
        Inputmask({'mask': '(999) 999-9999'}).mask(selector);        
      }, 0);
    }
    else
    {
      this.registroForm.controls['tipo_registro'].setValue(this.tipoRegistro);
    }
  }

  registro( tipo: string )
  {
    this.tipoRegistro = tipo;

    switch (tipo) {
      case 'Usuario':
        this.usuario = true;
        this.profesional = false;
        this.empresa = false;
        this.continuar = false;
        break;
      case 'Profesional':
        this.usuario = false;
        this.profesional = true;
        this.empresa = false;
        this.continuar = false;
        break;
      case 'Empresa':
        this.usuario = false;
        this.profesional = false;
        this.empresa = true;
        this.continuar = false;
        break;
      default:
        this.empresa = false;
        this.profesional = false;
        this.usuario = false;
        this.continuar = false;
        break;
    }    
  }



}
