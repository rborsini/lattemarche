<template>
  <div>
    <table class="table table-hover table-striped table-bordered">
      <div class="d-none">
        <slot class="toolbox" name="toolbox"></slot>
      </div>
      <thead>
        <tr>
          <slot name="thead"></slot>
        </tr>
      </thead>
      <tbody></tbody>
      <tfoot>
        <tr>
          <slot name="tfoot"></slot>
        </tr>
      </tfoot>
    </table>
  </div>
</template>

<script>
var table = null;

import dataTable from "datatables.net-bs4";
import $ from "jquery";

export default {
  props: ["options", "rows"],

  mounted: function() {},

  watch: {
    options: function(options) {
      this.init(options);
    },

    rows: function(rows) {
      table.clear();
      table.rows.add(rows);
      table.draw();
    }
  },

  methods: {
    init: function(options) {
      var vm = this;

      // https://datatables.net/reference/option/dom
      var dom = "";

      if ($(".toolbox")[0])
        dom = '<"top row"<"col-6 float-left"f><"col-6 float-right toolbox-div">>t<"row"<"col-6"l><"col-6"p>>';
      else 
        dom = '<"row"<"col-6"f><"col-6">>t<"row"<"col-6"l><"col-6"p>>';

      var defaultOptions = {
        dom: dom,
        initComplete: function() {
          if ($(".toolbox")[0]) $(".toolbox-div")[0].append($(".toolbox")[0]);

          // rimuovo classe css custom-select
          $("#DataTables_Table_0_length select").removeClass("custom-select");
        },
        ajax: options.ajax,
        rowGroup: options.rowGroup,
        columns: options.columns,
        serverSide: options.serverSide ? false : options.serverSide,
        paging: true,
        lengthMenu: [
          [20, 50, 100, 500, 1000, -1],
          [20, 50, 100, 500, 1000, "All"]
        ],
        pageLength: 20,        
        language: {
          sEmptyTable: "Nessun dato presente",
          sInfo: "_START_ - _END_ di _TOTAL_",
          sInfoEmpty: "",
          sInfoFiltered: "(filtrati da _MAX_ righe totali)",
          sInfoPostFix: "",
          sInfoThousands: ",",
          sLengthMenu: "_MENU_",
          sLoadingRecords: "Caricamento...",
          sProcessing: "Elaborazione...",
          sSearch: "Cerca:",
          sZeroRecords: "Nessun dato presente",
          oPaginate: {
            sFirst: "Inizio",
            sPrevious: "Precedente",
            sNext: "Successivo",
            sLast: "Fine"
          },
          oAria: {
            sSortAscending:
              ": attiva per ordinare la colonna in ordine crescente",
            sSortDescending:
              ": attiva per ordinare la colonna in ordine decrescente"
          }
        }
      };

      // merge delle opzioni
      var fullOptions = Object.assign(defaultOptions, options);

      table = $(this.$el.children[0]).DataTable(fullOptions);

      table.on("draw.dt", function() {
        vm.$emit("data-loaded");
      });
    },

    load: function(paramsQueryString) {
      var baseUrl = table.ajax.url().split("?")[0];
      table.ajax.url(baseUrl + "?" + paramsQueryString).load();
    }
  }
};
</script>

<style>
@import "../../public/plugins/datatables/css/dataTable.b4.css";
</style>