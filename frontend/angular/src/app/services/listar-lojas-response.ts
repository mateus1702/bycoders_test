import { BaseResponse } from "./base-response";

export interface Loja {
    id: number;
    nome: string;
    nomeRepresentante: string;
}

export interface ListarLojasResponse extends BaseResponse {
    lojas: Loja[];
}