<template>
  <div class="modal fade"  tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" >
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Dettaglio categoria</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="modal-body">

          <!--  Pagina -->
          <div class="row form-group">
            <label class="col-2">Pagina</label>
            <div class="col-10">
              <input type="text" class="form-control" v-model="attachmentCategory.Page">
            </div>
          </div>

          <!--  Descrizione -->
          <div class="row form-group">
            <label class="col-2">Descrizione</label>
            <div class="col-10">
              <input type="text" class="form-control" v-model="attachmentCategory.Description">
            </div>
          </div>

        </div>
        <div class="modal-footer">
          <button type="button" v-on:click="onSave()" class="btn btn-primary" data-dismiss="modal" >Ok</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">

import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

import { AttachmentCategory } from "../../models/attachmentCategory.model";
import { AttachmentCategoriesService } from "../../services/attachmentCategories.service";

@Component({})
export default class App extends Vue {

  @Prop()
  public attachmentCategory!: AttachmentCategory;
  @Prop()
  public isNew!: boolean;

  private attachmentCategoriesService: AttachmentCategoriesService = new AttachmentCategoriesService();

  constructor() {
    super();
  }

  mounted() {
    
  }


  public onSave() {
    this.attachmentCategoriesService
      .save(this.attachmentCategory)
      .then(response => {
        if (response.data != undefined) {
          this.$emit("saved");
          this.close();
        } else {
          // save KO!!
          this.$emit("error");
          this.close();
        }
      });
  }

  public open(): void {
    $(this.$el).modal("show");
  }

  public close(): void {
    $(this.$el).modal("hide");
  }
}
</script>

<style>
.modal-header {
  display: flex;
}
</style>
