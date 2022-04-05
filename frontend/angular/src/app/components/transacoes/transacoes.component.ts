import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ListarLojasResponse } from 'src/app/services/listar-lojas-response';
import { ListarTransacoesRequest } from 'src/app/services/listar-transacoes-request';
import { ListarTransacoesResponse, Transacao } from 'src/app/services/listar-transacoes-response';
import { LojasService } from 'src/app/services/lojas.service';

@Component({
  selector: 'app-transacoes',
  templateUrl: './transacoes.component.html',
  styleUrls: ['./transacoes.component.css']
})
export class TransacoesComponent implements OnInit {

  transacoes: Transacao[] = [];
  saldo: number = 0;

  constructor(private lojasService: LojasService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    let lojaId: string = this.route.snapshot.params["lojaId"];

    let request: ListarTransacoesRequest = {
      lojaId: parseInt(lojaId)
    };
    this.lojasService.listarTransacoes(request).subscribe((response: ListarTransacoesResponse) => {
      this.transacoes = response.transacoes;
      this.saldo = response.saldo;
      console.log(this.saldo);
    });
  }

}
