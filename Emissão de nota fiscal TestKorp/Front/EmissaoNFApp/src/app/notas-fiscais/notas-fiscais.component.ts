import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

type ProdutoDisponivel = {
  id?: number;
  codigo: string;
  nome: string;
  quantidade: number;
  quantidadeSelecionada?: number;
};

@Component({
  selector: 'app-notas-fiscais',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule],
  templateUrl: './notas-fiscais.component.html',
  styleUrls: ['./notas-fiscais.component.css']
})
export class NotasFiscaisComponent {
  notasFiscais: any[] = [];
  produtosDisponiveis: ProdutoDisponivel[] = [];
  creating = false;

  private produtosApi = 'https://localhost:7043/Api/Produto';
  private notasApi = 'https://localhost:7043/api/NotaFiscal';

  constructor(private http: HttpClient) {
    this.loadProdutos();
  }

  novaNotaFiscal = {
    numero: 0,
    status: 'Aberta',
    produtos: [] as any[]
  };

  adicionarNotaFiscal() {
    if (!this.isNotaValida()) {
      alert('Valide as quantidades dos produtos antes de criar a nota fiscal.');
      return;
    }
    const produtosPayload = this.novaNotaFiscal.produtos.map(p => ({
      produtoId: p.id,
      quantidade: p.quantidadeSelecionada
    }));
    const payload = {
      status: this.novaNotaFiscal.status || 'Aberta',
      produtos: produtosPayload
    };

    this.creating = true;
    this.http.post(this.notasApi, payload)
      .subscribe((res: any) => {
        const created = res;
        if (created && created.produtos) {
          created.produtos = created.produtos.map((p: any) => {
            const prod = this.produtosDisponiveis.find(pd => pd.id === p.produtoId || pd.codigo === p.produtoId || pd.codigo === String(p.produtoId));
            return {
              produtoId: p.produtoId,
              nome: prod ? prod.nome : String(p.produtoId),
              quantidade: p.quantidade
            };
          });
        }
        this.notasFiscais.push(created);
        this.novaNotaFiscal = { numero: 0, status: 'Aberta', produtos: [] };
        this.loadProdutos();
        this.loadNotas();
        this.creating = false;
      }, (err) => {
        console.error('Erro ao criar nota fiscal:', err);
        alert('Erro ao criar nota fiscal. Veja console para detalhes.');
        this.creating = false;
      });
  }

  selecionarProduto(event: any, produto: any) {
    if (event.target.checked) {
      if (produto.quantidadeSelecionada === undefined) produto.quantidadeSelecionada = 1;
      this.novaNotaFiscal.produtos.push(produto);
    } else {
      this.novaNotaFiscal.produtos = this.novaNotaFiscal.produtos.filter(
        (p) => p.codigo !== produto.codigo
      );
    }
  }

  isNotaValida(): boolean {
    if (!this.novaNotaFiscal.produtos || this.novaNotaFiscal.produtos.length === 0) return false;
    for (const p of this.novaNotaFiscal.produtos) {
      const qtd = Number(p.quantidadeSelecionada) || 0;
      if (qtd <= 0) return false;
      const prodDisponivel = this.produtosDisponiveis.find(pd => pd.codigo === p.codigo || pd.id === p.id);
      if (!prodDisponivel) return false;
      if (qtd > prodDisponivel.quantidade) return false;
    }
    return true;
  }

  imprimirNotaFiscal(nota: any) {
    if (nota.status !== 'Aberta') {
      alert('Apenas notas fiscais com status "Aberta" podem ser impressas.');
      return;
    }
    const original = nota.status;
    nota.processing = true;
    this.http.post(`${this.notasApi}/${nota.numero}/fechar`, null)
      .subscribe(() => {
        nota.status = 'Fechada';
        nota.processing = false;
        this.loadProdutos();
      }, (err) => {
        nota.processing = false;
        console.error('Erro ao fechar nota:', err);
        alert('Erro ao fechar nota. Veja console para detalhes.');
        nota.status = original;
      });
  }

  private loadProdutos() {
    this.http.get<any[]>(this.produtosApi)
      .subscribe((data) => {
        this.produtosDisponiveis = data.map(d => ({
          id: d.id,
          codigo: d.codigo,
          nome: d.nome,
          quantidade: d.quantidade,
          quantidadeSelecionada: 0
        }));
        this.loadNotas();
      }, (err) => {
        console.error('Erro ao carregar produtos disponíveis:', err);
      });
  }

  private loadNotas() {
    this.http.get<any[]>(this.notasApi + '/GetAll')
      .subscribe((data) => {
        this.notasFiscais = data.map(n => ({
          ...n,
          produtos: n.produtos.map((p: any) => ({
            produtoId: p.produtoId,
            nome: (this.produtosDisponiveis.find(pd => pd.id === p.produtoId) || { nome: String(p.produtoId) }).nome,
            quantidade: p.quantidade
          }))
        }));
      }, (err) => {
        
      });
  }
}
