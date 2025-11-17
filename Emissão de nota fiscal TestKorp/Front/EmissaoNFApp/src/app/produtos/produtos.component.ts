import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule],
  templateUrl: './produtos.component.html',
  styleUrls: ['./produtos.component.css']
})
export class ProdutosComponent {
  produtos: { codigo: string; nome: string; quantidade: number }[] = [];
  novoProduto = { codigo: '', nome: '', quantidade: 0 };

  private apiUrl = 'https://localhost:7043/Api/Produto';

  constructor(private http: HttpClient) {
    this.buscarProdutos();
  }

  buscarProdutos() {
  this.http.get<{ codigo: string; nome: string; quantidade: number }[]>(this.apiUrl)
    .subscribe(
      (data) => {
        console.log('Produtos recebidos:', data); 
        this.produtos = data;
      },
      (error) => {
        console.error('Erro ao buscar produtos:', error); 
      }
    );
}

  adicionarProduto() {
  console.log('Enviando produto:', this.novoProduto); 
  this.http.post(this.apiUrl, this.novoProduto)
    .subscribe(() => {
      this.produtos.push({ ...this.novoProduto });
      this.novoProduto = { codigo: '', nome: '', quantidade: 0 };
    }, (error) => {
      console.error('Erro ao adicionar produto:', error); 
    });
}
}
