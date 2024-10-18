import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CepComponent } from './pages/cep/cep.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'cep/pesquisaCep',
    pathMatch: 'full',
  },
  {
    path: 'cep/pesquisaCep',
    component: CepComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
