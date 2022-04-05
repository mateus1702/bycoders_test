import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';

import {HttpClientModule} from '@angular/common/http';
import { UploadComponent } from './components/upload/upload.component';
import { LojasComponent } from './components/lojas/lojas.component';
import { TransacoesComponent } from './components/transacoes/transacoes.component';

@NgModule({
  declarations: [
    AppComponent,
    FileUploadComponent,
    UploadComponent,
    LojasComponent,
    TransacoesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
