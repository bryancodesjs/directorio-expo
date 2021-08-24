import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-administrativos',
  templateUrl: './administrativos.component.html',
  styleUrls: ['./administrativos.component.scss']
})
export class AdministrativosComponent implements OnInit {
  @Input() administradores: any[] = [];
  @Output() privilegiosEvent = new EventEmitter();
  ultimoSeleccionado: number = 0;

  constructor() { }

  ngOnInit(): void 
  {
  }

  Privilegios( id: number, artista: string )
  {
    (document.getElementById('data-'+ id) as HTMLElement).className = 'btn btn-outline-danger';
    if (this.ultimoSeleccionado == 0) 
    {
      this.ultimoSeleccionado = id;
    }
    else
    {
      (document.getElementById('data-'+ this.ultimoSeleccionado) as HTMLElement).className = 'btn btn-primary';
      this.ultimoSeleccionado = id;
    }
    
    let obj = {
      id,
      artista
    }
    this.privilegiosEvent.emit(obj);
  }

}
