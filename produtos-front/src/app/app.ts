import { CurrencyPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Produto, ProdutoInput } from './models/produto.model';
import { ProdutosService } from './services/produtos.service';

interface NovoProdutoForm {
  nome: string;
  preco: number | null;
  quantidade: number | null;
}

@Component({
  imports: [CurrencyPipe, FormsModule],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit {
  protected produtos: Produto[] = [];
  protected buscaId = '';
  protected produtoEncontrado = signal<Produto | null>(null);
  protected novoProduto: NovoProdutoForm = { nome: '', preco: null, quantidade: null };
  protected produtoEmEdicaoId: number | null = null;
  protected produtoEmEdicao: ProdutoInput = { nome: '', preco: 0, quantidade: 0 };
  protected carregando = false;
  protected enviando = false;
  protected mensagemErro = signal('');
  protected mensagemSucesso = signal('');

  constructor(
    private readonly produtosService: ProdutosService,
    private readonly changeDetector: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.carregarProdutos();
  }

  protected carregarProdutos(): void {
    this.carregando = true;
    this.mensagemErro.set('');
    this.produtosService.listarTodos().subscribe({
      next: (produtos) => {
        this.produtos = produtos;
        this.carregando = false;
        this.changeDetector.markForCheck();
      },
      error: (error: unknown) => {
        this.mensagemErro.set(this.mensagemDaFalha(error, 'Não foi possível carregar os produtos.'));
        this.carregando = false;
        this.changeDetector.markForCheck();
      },
    });
  }

  protected buscarPorId(): void {
    const id = Number(this.buscaId);
    this.limparMensagens();
    this.produtoEncontrado.set(null);

    if (!Number.isInteger(id) || id <= 0) {
      this.mensagemErro.set('Informe um ID inteiro maior que zero.');
      return;
    }

    this.produtosService.buscarPorId(id).subscribe({
      next: (produto) => {
        this.produtoEncontrado.set(produto);
        this.mensagemSucesso.set(`Produto #${produto.id} encontrado.`);
        this.changeDetector.markForCheck();
      },
      error: (error: unknown) => {
        this.mensagemErro.set(this.mensagemDaFalha(error, 'Não foi possível buscar o produto.'));
        this.changeDetector.markForCheck();
      },
    });
  }

  protected adicionarProduto(form: NgForm): void {
    this.limparMensagens();
    const { nome, preco, quantidade } = this.novoProduto;
    if (
      form.invalid ||
      !nome.trim() ||
      preco === null ||
      !Number.isFinite(preco) ||
      preco <= 0 ||
      quantidade === null ||
      !Number.isInteger(quantidade) ||
      quantidade < 0
    ) {
      form.control.markAllAsTouched();
      this.mensagemErro.set('Preencha o nome, informe um preço maior que zero e verifique a quantidade.');
      return;
    }

    const produto: ProdutoInput = { nome: nome.trim(), preco, quantidade };
    this.enviando = true;
    this.produtosService.criar(produto).subscribe({
      next: (produtoCriado) => {
        form.resetForm();
        this.novoProduto = { nome: '', preco: null, quantidade: null };
        this.mensagemSucesso.set(`Produto "${produtoCriado.nome}" adicionado com sucesso.`);
        this.enviando = false;
        this.carregarProdutos();
        this.changeDetector.markForCheck();
      },
      error: (error: unknown) => {
        this.mensagemErro.set(this.mensagemDaFalha(error, 'Não foi possível adicionar o produto.'));
        this.enviando = false;
        this.changeDetector.markForCheck();
      },
    });
  }

  protected iniciarEdicao(produto: Produto): void {
    this.limparMensagens();
    this.produtoEmEdicaoId = produto.id;
    this.produtoEmEdicao = {
      nome: produto.nome,
      preco: produto.preco,
      quantidade: produto.quantidade,
    };
  }

  protected cancelarEdicao(): void {
    this.produtoEmEdicaoId = null;
  }

  protected salvarEdicao(id: number): void {
    this.limparMensagens();
    this.enviando = true;
    this.produtosService.atualizar(id, this.produtoEmEdicao).subscribe({
      next: (produto) => {
        this.produtoEmEdicaoId = null;
        if (this.produtoEncontrado()?.id === produto.id) {
          this.produtoEncontrado.set(produto);
        }
        this.mensagemSucesso.set(`Produto "${produto.nome}" atualizado com sucesso.`);
        this.enviando = false;
        this.carregarProdutos();
        this.changeDetector.markForCheck();
      },
      error: (error: unknown) => {
        this.mensagemErro.set(this.mensagemDaFalha(error, 'Não foi possível atualizar o produto.'));
        this.enviando = false;
        this.changeDetector.markForCheck();
      },
    });
  }

  protected removerProduto(produto: Produto): void {
    this.limparMensagens();
    this.produtosService.remover(produto.id).subscribe({
      next: () => {
        this.mensagemSucesso.set(`Produto "${produto.nome}" removido com sucesso.`);
        if (this.produtoEncontrado()?.id === produto.id) {
          this.produtoEncontrado.set(null);
        }
        this.carregarProdutos();
        this.changeDetector.markForCheck();
      },
      error: (error: unknown) => {
        this.mensagemErro.set(this.mensagemDaFalha(error, 'Não foi possível remover o produto.'));
        this.changeDetector.markForCheck();
      },
    });
  }

  private limparMensagens(): void {
    this.mensagemErro.set('');
    this.mensagemSucesso.set('');
  }

  private mensagemDaFalha(error: unknown, fallback: string): string {
    if (error instanceof HttpErrorResponse) {
      const mensagem = (error.error as { message?: unknown } | null)?.message;
      if (typeof mensagem === 'string') {
        return mensagem;
      }

      if (error.status === 0) {
        return 'Não foi possível conectar à API. Verifique se ela está em execução.';
      }

      if (error.status === 404) {
        return 'Produto não encontrado.';
      }

      return `${fallback} (HTTP ${error.status}).`;
    }

    return fallback;
  }
}
