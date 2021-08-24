import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BackofficeComponent } from './backoffice/backoffice.component';
import { ConfiguracionComponent } from './components/configuracion/configuracion.component';
import { LoginComponent } from './components/login/login.component';
import { RegistroComponent } from './components/registro/registro.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { GestionUsuariosComponent } from './gestion-usuarios/gestion-usuarios.component';
// PARA PROTEGER LAS RUTAS QUE NECESITEN ESTAR LOGUEADO
import { AuthGuard as AuthorizeUser } from './middleware/auth/auth.guard';
import { AuthGuard as AuthorizeAdmin } from './middleware/auth/authorize.guard';
import { VistaPerfilComponent } from './vista-perfil/vista-perfil.component';
import { MensajeriaComponent } from './mensajeria/mensajeria.component';
import { MensajeriaGuard } from './middleware/auth/mensajeria.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },  
  { path: 'registro', component: RegistroComponent },
  { path: 'perfil/:id', component: VistaPerfilComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthorizeUser] },
  { path: 'backoffice', component: BackofficeComponent, canActivate: [AuthorizeAdmin] },
  { path: 'configuracion', component: ConfiguracionComponent, canActivate: [AuthorizeUser] },
  { path: 'gestion-usuarios', component: GestionUsuariosComponent, canActivate: [AuthorizeAdmin]},
  { path: 'mensajeria', component: MensajeriaComponent, canActivate: [MensajeriaGuard] },
  { path: 'mensajeria/:id', component: MensajeriaComponent, canActivate: [MensajeriaGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
