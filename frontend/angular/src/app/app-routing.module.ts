import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LojasComponent } from './components/lojas/lojas.component';
import { TransacoesComponent } from './components/transacoes/transacoes.component';
import { UploadComponent } from './components/upload/upload.component';

const routes: Routes = [
  { path: '', redirectTo: '/upload', pathMatch: 'full' },
  { path: 'upload', component: UploadComponent },
  { path: 'lojas', component: LojasComponent },
  { path: 'transacoes/:lojaId', component: TransacoesComponent },
  { path: '**', redirectTo: '/upload', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
