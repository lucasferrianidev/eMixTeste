import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Cep } from '../../cep';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CepService {
  itensPorPagina = 5;

  private API = 'https://localhost:7102/Cep';

  constructor(private http: HttpClient) {}

  buscarEnderecoPorCep(cep: number): Observable<Cep> {
    const url = `${this.API}/${cep}`;
    return this.http.get<Cep>(url);
  }

  buscarCepsPorLogradouro(logradouro: string): Observable<Cep[]> {
    const url = `${this.API}/logradouro/${logradouro}`;
    return this.http.get<Cep[]>(url);
  }

  verCepsPorUf(uf: string): Observable<Cep[]> {
    const url = `${this.API}/uf/${uf}`;
    return this.http.get<Cep[]>(url);
  }

  // listarEspecialidade(pagina: number, filtro: string): Observable<Especialidade[]> {

  //   let params = new HttpParams()
  //     .set('page', pagina)
  //     .set('perPage', this.itensPorPagina)

  //   if (filtro.length > 2) {
  //     params = params.set('query', filtro)
  //   }

  //   return this.http.get<Especialidade[]>(this.API, { params })
  // }

  // criarEspecialidade(especialidade: Especialidade): Observable<Especialidade> {
  //   return this.http.post<Especialidade>(this.API, especialidade)
  // }

  // editarEspecialidade(especialidade: Especialidade): Observable<Especialidade> {
  //   const url = `${this.API}/${especialidade.cdEspecialidade}`
  //   return this.http.put<Especialidade>(url, especialidade)
  // }

  // excluirEspecialidade(especialidade: Especialidade): Observable<Especialidade> {
  //   const url = `${this.API}/${especialidade.cdEspecialidade}`
  //   return this.http.delete<Especialidade>(url)
  // }
}
