import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-backoffice-nav',
  templateUrl: './backoffice-nav.component.html',
  styleUrls: ['./backoffice-nav.component.scss']
})
export class BackofficeNavComponent implements OnInit {
  @Output() mensajes = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void 
  {
  }

  Mensajes()
  {
    this.mensajes.emit();
  }

}
