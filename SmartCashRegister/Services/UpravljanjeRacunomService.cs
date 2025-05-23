﻿using iTextSharp.text.pdf;
using iTextSharp.text;
using SmartCashRegister.Models;
using SmartCashRegister.Services.Interfaces;
using System.IO;
using System.Windows;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartCashRegister.Services
{
    public class UpravljanjeRacunomService : IUpravljanjeRacunomService
    {
        private readonly IPristupBaziService _dbPristup;
        private readonly IPretragaRacunaService _pretragaRacunaService;

        public UpravljanjeRacunomService(IPristupBaziService dbPristup,IPretragaRacunaService pretragaRacunaService)
        {
            _dbPristup = dbPristup;
            _pretragaRacunaService= pretragaRacunaService;
        }
        public bool IzveziUPDF(Racun racun)
        {
            try
            {

                string filePath = $"Racun_{racun.RacunId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var paperSize = new iTextSharp.text.Rectangle(210, 800);
                Document document = new Document(paperSize, 10, 10, 10, 10);
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                string osobaQuery = $"SELECT ime, prezime FROM Osoba WHERE osoba_id = {racun.OsobaId}";
                DataTable osobaResult = _dbPristup.ExecuteQuery(osobaQuery);

                string imePrezime = "";
                if (osobaResult.Rows.Count > 0)
                {
                    var row = osobaResult.Rows[0];
                    imePrezime = row["ime"] + " " + row["prezime"];
                }

                document.Add(new Paragraph("SmartCash Register DOO"));
                document.Add(new Paragraph("------------------------------------------"));
                document.Add(new Paragraph($"Račun br: {racun.RacunId}"));
                document.Add(new Paragraph($"Datum: {racun.Datum:dd.MM.yyyy HH:mm}"));
                document.Add(new Paragraph($"Blagajnik: {imePrezime}"));
                document.Add(new Paragraph("------------------------------------------"));

                var stavke = _pretragaRacunaService.PrikaziStavkeRacuna(racun.RacunId);
                foreach (var stavka in stavke)
                {
                    document.Add(new Paragraph($"{stavka.Naziv}"));
                    document.Add(new Paragraph($" {stavka.Kolicina} x {stavka.Cena:F2} RSD\t= {stavka.Ukupno:F2}"));
                }

                document.Add(new Paragraph("------------------------------------------"));
                document.Add(new Paragraph($"Ukupno: {racun.Cena:F2} RSD"));
                document.Add(new Paragraph("------------------------------------------"));
                document.Close();

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri izvozu računa u PDF: " + ex.Message);
                return false;
            }
            
        }
        public bool StornirajRacun(Racun racun)
        {
            string query = $"UPDATE Racun SET status = 'Storniran' WHERE racun_id = {racun.RacunId} AND status LIKE 'Aktivan'";

            int affected = _dbPristup.ExecuteNonQuery(query);

            if (affected == 0) 
                return false;
            return true;
        }
        public bool ObrisiRacun(Racun racun)
        {
            string stavkaQuery = "DELETE FROM StavkaRacuna WHERE racun_id = @racunId";
            SqlParameter[] parameters = {
                new SqlParameter("@racunId", racun.RacunId)
            };
            _dbPristup.ExecuteNonQuery(stavkaQuery, parameters);

            SqlParameter[] parametersR = {
                new SqlParameter("@racunId", racun.RacunId)
            };
            string racunQuery = "DELETE FROM Racun WHERE racun_id = @racunId";
            _dbPristup.ExecuteNonQuery(racunQuery, parametersR);

            MessageBox.Show($"Račun sa ID: {racun.RacunId} uspešno obrisan.");
            return true;
        }
    }
}
