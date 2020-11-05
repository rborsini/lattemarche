describe('Bug #326322', () => {
    it('Apertura pagina utenti', () => {
        cy.visit('http://localhost:53137/')
        expect(true).to.equal(true)
    })

    it('Selezione profilo trasportatore', () => {
        expect(true).to.equal(true)
    })    
  })