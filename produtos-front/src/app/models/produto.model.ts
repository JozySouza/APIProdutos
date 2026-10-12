export interface Produto {
  id: number;
  nome: string;
  preco: number;
  quantidade: number;
}

export interface ProdutoInput {
  nome: string;
  preco: number;
  quantidade: number;
}
