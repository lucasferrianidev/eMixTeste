import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CepService } from './cep.service';
import { Cep } from 'src/app/cep';

@Component({
  selector: 'cep-root',
  templateUrl: './cep.component.html',
  styleUrls: ['./cep.component.scss'],
})
export class CepComponent implements OnInit {
  showLocalizaPorLogradouro: boolean = false;
  showEndereco: boolean = false;
  showLogradourosLocalizados: boolean = false;
  showVerCepsPorUf: boolean = false;

  cepsLogradouro: Cep[] = [];
  cepsPorUf: Cep[] = [];

  cepForm!: FormGroup;
  enderecoForm!: FormGroup;

  logradouro!: string;
  complemento!: string;
  bairro!: string;
  localidade!: string;
  uf!: string;
  unidade!: string;
  ibge!: string;
  gia!: string;

  constructor(private formBuilder: FormBuilder, private service: CepService) {}

  ngOnInit(): void {
    this.cepForm = this.formBuilder.group({
      cep: [null, [Validators.required]],
      logradouro: [null, []],
      uf: [null, []],
    });
    this.enderecoForm = this.formBuilder.group({
      complemento: [null, []],
      bairro: [null, []],
      localidade: [null, []],

      unidade: [null, []],
      ibge: [null, []],
      gia: [null, []],
    });
  }

  pesquisar() {
    // const cep = this.cepForm.value.cep.replace(/[^0-9]/g, '');
    this.service.buscarEnderecoPorCep(this.cepForm.value.cep).subscribe(
      (res) => {
        this.showEndereco =
          res.cep !== null && res.cep !== undefined && res.cep !== '';
        this.showLogradourosLocalizados = false;
        this.showLocalizaPorLogradouro = false;
        this.showVerCepsPorUf = false;
        this.logradouro = res.logradouro;
        this.complemento = res.complemento;
        this.bairro = res.bairro;
        this.localidade = res.localidade;
        this.uf = res.uf;
        this.unidade = res.unidade.toString();
        this.ibge = res.ibge.toString();
        this.gia = res.gia;
        this.enderecoForm.patchValue({
          logradouro: res.logradouro,
        });
        console.log(res);
      },
      (error) => {
        console.log(error);
        this.showEndereco = false;
        if (error.status === 400) {
          if (error.error === 'Inexistente') alert('CEP inexistente!');
          else alert('CEP inválido!');
        } else if (error.status === 500) alert('Problema encontrado');
      }
    );
  }

  pesquisarLogradouro() {
    if (
      !this.showLocalizaPorLogradouro &&
      window.confirm('Deseja consultar se logradouro existe na base?')
    )
      this.showLocalizaPorLogradouro = true;
    else {
      // console.error(this.cepForm.value.logradouro);
      this.service
        .buscarCepsPorLogradouro(this.cepForm.value.logradouro)
        .subscribe((res) => {
          console.log(res);
          this.cepsLogradouro = res;
        });
    }
  }

  verCepsPorUf() {
    if (
      !this.showVerCepsPorUf &&
      window.confirm(
        'Deseja visualizar todos os CEPs de uma UF? Se sim, digite no campo.'
      )
    )
      this.showVerCepsPorUf = true;
    else {
      // console.error(this.cepForm.value.logradouro);
      this.service.verCepsPorUf(this.cepForm.value.uf).subscribe(
        (res) => {
          console.log(res);
          this.cepsPorUf = res;
        },
        (error) => {
          console.log(error);
          this.showEndereco = false;
          this.showLocalizaPorLogradouro = false;
          if (error.status === 400) alert('UF inválida!');
          else if (error.status === 500) alert('Problema encontrado');
        }
      );
    }
  }
}
