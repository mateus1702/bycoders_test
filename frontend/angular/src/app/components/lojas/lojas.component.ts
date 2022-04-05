import { Component, OnInit } from '@angular/core';
import { ListarLojasResponse, Loja } from 'src/app/services/listar-lojas-response';
import { LojasService } from 'src/app/services/lojas.service';

@Component({
  selector: 'app-lojas',
  templateUrl: './lojas.component.html',
  styleUrls: ['./lojas.component.css']
})
export class LojasComponent implements OnInit {

  lojas: Loja[] = [];

  constructor(private lojasService: LojasService) { }

  ngOnInit(): void {
    this.lojasService.listarLojas().subscribe((response: ListarLojasResponse) => {
      this.lojas = response.lojas;
    })
  }
}
