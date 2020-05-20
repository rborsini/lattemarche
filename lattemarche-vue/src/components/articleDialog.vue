<template>
  <div id="modal-article-dialog" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" >
    <div class="modal-dialog modal-lg article-modal">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Dettaglio articolo</h5>
          <button type="button" class="close" data-dismiss="modal" v-on:click="$emit('cancel')" aria-label="Close" >
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body pl-5 pr-5">
          <!-- articolo -->
          <div class="row form-group">
            <label class="col-2">Descrizione</label>
            <div class="col-10">
              <select2 ref="articleSelect" :disabled="!editable" :dropdownparent="'#modal-article-dialog'" class="form-control" :placeholder="''" :options="articles" :value.sync="articleId" :value-field="'Id'" v-on:value-changed="onArticleSelected()" :text-field="'Description'" />
            </div>
          </div>

          <div class="row form-group">
            <!-- Qta -->
            <label class="col-2">Quantità</label>
            <div class="col-1">
              <input :disabled="!editable" type="number" class="form-control" v-model="quantity" v-on:change="onQuantityChanged" />
            </div>

            <!-- UOM -->
            <label class="col-1">Un. Misura</label>
            <div class="col-1">
              <input :disabled="!editable" class="form-control" v-model="articleUOM" />
            </div>

            <!-- Prezzo unitario -->
            <label class="col-1">Prezzo (€)</label>
            <div class="col-2">
                  <input :disabled="!editable" class="form-control" v-model="articlePrice" v-on:change="onUnitPriceChanged" />
            </div>

            <!-- Prezzo totale -->
            <label class="col-2">Importo tot (€)</label>
            <div class="col-2">
                  <input :disabled="!editable" class="form-control" v-model="totalPrice" />
            </div>            
          </div>

          <!-- Note -->
          <div class="row">
            <label class="col-2">Note</label>
            <div class="col-10 text-right">
              <textarea class="form-control" v-model="note" rows="3"></textarea>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary mr-2" data-dismiss="modal" v-on:click="$emit('cancel')" >Annulla</button>
          <button :disabled="!articleId || !editable" class="btn btn-primary" v-on:click="onSave()" >Ok</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Prop, Watch, Emit, Vue, Component } from "vue-property-decorator";
import Select2 from "./select2.vue";

import { ArticlesService } from "../services/articles.service";
import { DocumentItem } from "@/models/documentItem.model";
import { Article, ArticleSearchModel } from "@/models/article.model";

@Component({
  components: {
    Select2
  }
})
export default class ArticleDialog extends Vue {
  $refs: any = {
    articleSelect: Vue
  };

  @Prop() public item!: DocumentItem;
  @Prop() public dateDocument!: string;
  @Prop() public editable!: boolean;

  private articles: Article[] = [];

  private articleCode: string = "";
  private articleId: string = "";
  private articleUOM: string = "";
  private articleDescription: string = "";
  private articlePrice: number = 0;
  private totalPrice: number = 0;
  private note: string = "";

  private quantity: number = 1;

  private articlesService: ArticlesService = new ArticlesService();

  constructor() {
    super();
  }

  // salva item
  public onSave() {
    this.item.ArticleCode = this.articleCode;
    this.item.ArticleId = this.articleId;
    this.item.UOM = this.articleUOM;
    this.item.Description = this.articleDescription;
    this.item.UnitPrice = this.articlePrice;
    this.item.TotalPrice = this.totalPrice;

    this.item.SuggestedPrice = this.articles.filter(
      a => a.Id == this.articleId
    )[0].Price;

    this.item.Quantity = this.quantity;
    this.item.Note = this.note;

    this.close();
    this.$emit("saved", this.item);
  }

  public onArticleSelected() {
    var article: Article = this.articles.filter(a => a.Id == this.articleId)[0];

    this.articleCode = article != null ? article.ArticleCode : "";
    this.articleDescription = article != null ? article.Description : "";
    this.articleUOM = article != null ? article.UOM : "";
    this.articlePrice = article != null && article.Price != null ? article.Price : this.articlePrice;
    this.totalPrice = this.articlePrice * this.quantity;

  }

  public onQuantityChanged() {
    this.totalPrice = this.articlePrice * this.quantity;
  }

  public onUnitPriceChanged() {
    this.totalPrice = this.articlePrice * this.quantity;
  }

  // apertura modale
  public open(): void {
    var parameters: ArticleSearchModel = new ArticleSearchModel();
    if (this.dateDocument) {
      parameters.Date_Str = this.dateDocument;
    }
    this.articlesService.search(parameters).then(response => {
      this.articles = response.data;

      this.articleCode = this.item.ArticleCode;
      this.articleId = this.item.ArticleId;
      this.articleUOM = this.item.UOM;
      this.articleDescription = this.item.Description;
      this.articlePrice = this.item.UnitPrice;

      this.quantity = this.item.Quantity;
      this.totalPrice = this.item.TotalPrice;
      this.note = this.item.Note;
      $(this.$el).modal("show");
    });
  }

  // chiusura modale
  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>

<style>
.article-modal {
  max-width: 70%;
}

.modal-header {
  display: flex;
}
</style>

