import { BaseResponse } from "./base-response";

export interface ListarTransacoesResponse extends BaseResponse {
    transacoes: Transacao[];
    saldo: number;
}

export interface Transacao {
    id: number;
    tipoDeTransacao: string;
    dataFormatada: string;
    valor: number;
    cpf: string;
    numeroCartao: string;
    nomeLoja: string;
}