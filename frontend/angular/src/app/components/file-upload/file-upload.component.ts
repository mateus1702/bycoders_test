import { Component, OnInit } from '@angular/core';
import { FileUploadService } from './file-upload.service';
import { UploadResponse } from './upload-response';

@Component({
    selector: 'app-file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

    loading: boolean = false; // Flag variable
    file: File | undefined; // Variable to store file
    mensagem: string = "";
    arquivo: string = "";
    relatorio: string[] = [];

    // Inject service 
    constructor(private fileUploadService: FileUploadService) { }

    ngOnInit(): void {
    }

    onChange(event: any) {
        this.file = event.target.files[0];
        this.mensagem = "";
        this.arquivo = this.file?.name ?? "";
    }

    onUpload() {
        if (this.file == undefined)
            this.mensagem = "Nenhum arquivo selecionado.";
        else {
            this.mensagem = "Processando ...";

            this.fileUploadService.upload(this.file).subscribe(
                (response: UploadResponse) => {
                    this.loading = false;
                    this.mensagem = "Arquivo enviado com sucesso!";
                    this.relatorio = response.relatorioParser;
                }
            );
        }
    }

}
