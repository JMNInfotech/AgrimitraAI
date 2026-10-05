-- S002: reference/master data (idempotent)
insert into languages (code, name, native_name, sort_order) values
 ('en','English','English',1),('mr','Marathi','मराठी',2),('hi','Hindi','हिन्दी',3),
 ('gu','Gujarati','ગુજરાતી',4),('kn','Kannada','ಕನ್ನಡ',5),('te','Telugu','తెలుగు',6),
 ('ta','Tamil','தமிழ்',7),('bn','Bengali','বাংলা',8),('pa','Punjabi','ਪੰਜਾਬੀ',9)
on conflict (code) do nothing;
update languages set is_active = false where code not in ('en','mr','hi');

insert into soil_types (code, name, name_local) values
 ('black','Black (Regur)','{"mr":"काळी माती","hi":"काली मिट्टी"}'),
 ('red','Red','{"mr":"लाल माती","hi":"लाल मिट्टी"}'),
 ('alluvial','Alluvial','{"mr":"गाळाची माती","hi":"जलोढ़ मिट्टी"}'),
 ('laterite','Laterite','{"mr":"जांभी माती","hi":"लैटेराइट मिट्टी"}'),
 ('sandy','Sandy','{"mr":"वालुकामय माती","hi":"रेतीली मिट्टी"}'),
 ('clay','Clay','{"mr":"चिकणमाती","hi":"चिकनी मिट्टी"}'),
 ('loam','Loam','{"mr":"पोयटा माती","hi":"दोमट मिट्टी"}'),
 ('murum','Murum (gravelly)','{"mr":"मुरमाड माती","hi":"मुरुम मिट्टी"}')
on conflict (code) do nothing;

insert into irrigation_types (code, name, name_local) values
 ('drip','Drip','{"mr":"ठिबक सिंचन","hi":"ड्रिप सिंचाई"}'),
 ('sprinkler','Sprinkler','{"mr":"तुषार सिंचन","hi":"स्प्रिंकलर सिंचाई"}'),
 ('flood','Flood / Furrow','{"mr":"पाटपाणी","hi":"बाढ़ सिंचाई"}'),
 ('rainfed','Rainfed','{"mr":"कोरडवाहू","hi":"वर्षा आधारित"}'),
 ('other','Other','{}')
on conflict (code) do nothing;

insert into water_sources (code, name, name_local) values
 ('well','Open well','{"mr":"विहीर","hi":"कुआँ"}'),
 ('borewell','Borewell','{"mr":"बोअरवेल","hi":"नलकूप"}'),
 ('canal','Canal','{"mr":"कालवा","hi":"नहर"}'),
 ('river','River','{"mr":"नदी","hi":"नदी"}'),
 ('farm_pond','Farm pond','{"mr":"शेततळे","hi":"खेत तालाब"}'),
 ('lake','Lake / reservoir','{"mr":"तलाव","hi":"झील"}'),
 ('rain','Rain only','{"mr":"पावसावर","hi":"वर्षा"}')
on conflict (code) do nothing;

insert into expense_categories (code, name, name_local, sort_order) values
 ('seeds','Seeds','{"mr":"बियाणे","hi":"बीज"}',1),
 ('plants','Plants / Saplings','{"mr":"रोपे","hi":"पौध"}',2),
 ('fertilizer','Fertilizer','{"mr":"खते","hi":"उर्वरक"}',3),
 ('pesticide','Pesticides','{"mr":"कीटकनाशके","hi":"कीटनाशक"}',4),
 ('labour','Labour','{"mr":"मजुरी","hi":"मजदूरी"}',5),
 ('machinery','Machinery','{"mr":"यंत्रसामग्री","hi":"मशीनरी"}',6),
 ('irrigation','Irrigation','{"mr":"सिंचन","hi":"सिंचाई"}',7),
 ('electricity','Electricity','{"mr":"वीज","hi":"बिजली"}',8),
 ('transport','Transport','{"mr":"वाहतूक","hi":"परिवहन"}',9),
 ('consultant','Consultant','{"mr":"सल्लागार","hi":"सलाहकार"}',10),
 ('laboratory','Laboratory','{"mr":"प्रयोगशाळा","hi":"प्रयोगशाला"}',11),
 ('other','Other','{"mr":"इतर","hi":"अन्य"}',99)
on conflict (code) do nothing;

insert into soil_parameters (code, name, category, default_unit, min_plausible, max_plausible, sort_order) values
 ('ph','pH','reaction','pH',0,14,1),
 ('ec','Electrical Conductivity','salinity','dS/m',0,50,2),
 ('organic_carbon','Organic Carbon','organic','%',0,20,3),
 ('nitrogen','Available Nitrogen (N)','macro','kg/ha',0,2000,4),
 ('phosphorus','Available Phosphorus (P)','macro','kg/ha',0,500,5),
 ('potassium','Available Potassium (K)','macro','kg/ha',0,3000,6),
 ('sulphur','Sulphur (S)','secondary','mg/kg',0,500,7),
 ('calcium','Calcium (Ca)','secondary','mg/kg',0,50000,8),
 ('magnesium','Magnesium (Mg)','secondary','mg/kg',0,10000,9),
 ('zinc','Zinc (Zn)','micro','mg/kg',0,100,10),
 ('iron','Iron (Fe)','micro','mg/kg',0,500,11),
 ('manganese','Manganese (Mn)','micro','mg/kg',0,500,12),
 ('copper','Copper (Cu)','micro','mg/kg',0,100,13),
 ('boron','Boron (B)','micro','mg/kg',0,50,14),
 ('molybdenum','Molybdenum (Mo)','micro','mg/kg',0,10,15)
on conflict (code) do nothing;

insert into lab_test_types (code, name, category, sample_type) values
 ('soil_full','Soil Test (Full Panel)','soil','soil'),
 ('soil_basic','Soil Test (pH, EC, OC, NPK)','soil','soil'),
 ('soil_micronutrients','Soil Micronutrients','soil','soil'),
 ('water_irrigation','Irrigation Water Quality','water','water'),
 ('petiole_analysis','Petiole / Leaf Nutrient Analysis','plant_tissue','petiole'),
 ('residue_analysis','Pesticide Residue Analysis','pesticide_residue','fruit')
on conflict (code) do nothing;

insert into product_categories (code, name, domain, sort_order) values
 ('nursery_plants','Nursery Plants','nursery_plants',1),
 ('agri_products','Agriculture Products','agri_products',2),
 ('pesticides','Pesticides','pesticides',3),
 ('fertilizers','Fertilizers','fertilizers',4),
 ('seeds','Seeds','seeds',5),
 ('equipment','Equipment','equipment',6),
 ('soil_tests','Soil Tests','soil_tests',7),
 ('agri_services','Agricultural Services','agri_services',8),
 ('consultant_services','Consultant Services','consultant_services',9)
on conflict (code) do nothing;
insert into product_categories (parent_id, code, name, domain, sort_order)
select p.id, v.code, v.name, p.domain, v.so from product_categories p
join (values ('insecticides','Insecticides','pesticides',1),('fungicides','Fungicides','pesticides',2),('herbicides','Herbicides','pesticides',3),
             ('organic_fertilizers','Organic Fertilizers','fertilizers',1),('chemical_fertilizers','Chemical Fertilizers','fertilizers',2),
             ('bio_fertilizers','Bio Fertilizers','fertilizers',3)) as v(code, name, parent, so) on v.parent = p.code
on conflict (code) do nothing;

insert into ad_placements (code, name, allowed_ad_types, max_items) values
 ('home_banner','Home Banner','{banner,seasonal,product}',1),
 ('marketplace_banner','Marketplace Banner','{banner,product,nursery,laboratory}',2),
 ('crop_page','Crop Page','{product,sponsored_recommendation,in_feed}',2),
 ('product_page','Product Page','{product,in_feed}',2),
 ('ai_result_page','AI Result Page (clearly separated sponsored slot)','{sponsored_recommendation}',1),
 ('farm_dashboard','Farm Dashboard','{in_feed,product,seasonal}',1),
 ('search_results','Search Results','{product,in_feed,location_based}',2),
 ('sponsored_product_section','Sponsored Product Section','{product,nursery,laboratory,consultant_promotion}',4)
on conflict (code) do nothing;

insert into knowledge_categories (code, name) values
 ('crop_guides','Crop Guides'),('diseases','Diseases'),('pests','Pests'),('soil','Soil'),('weather','Weather'),
 ('irrigation','Irrigation'),('nutrition','Nutrition'),('faq','FAQs'),('products','Product Information')
on conflict (code) do nothing;

insert into system_configurations (config_key, value, value_type, description) values
 ('ai.disease.confidence_threshold','0.70','number','Below this confidence a scan becomes needs_expert_review'),
 ('calendar.occurrence_horizon_days','90','number','Rolling window for materialising recurring occurrences'),
 ('calendar.upcoming_window_hours','24','number','Scheduled → upcoming window'),
 ('auth.lockout.max_failed_attempts','5','number','Failed logins before lockout'),
 ('auth.lockout.duration_minutes','15','number','Base lockout duration'),
 ('auth.otp.ttl_seconds','300','number','OTP validity'),
 ('auth.access_token.ttl_minutes','10','number','Access token lifetime'),
 ('auth.refresh_token.ttl_days','30','number','Refresh token lifetime'),
 ('consultation.data_access_days','90','number','Consultant access to farmer data after a consultation'),
 ('ads.min_cohort_size','50','number','Minimum cohort size for aggregated ad analytics'),
 ('ads.sponsored_label.default','"Sponsored"','string','Mandatory label shown on every ad'),
 ('i18n.default_language','"en"','string','Fallback language')
on conflict (config_key) do nothing;

-- Crop master (starter set); stages for grapes, tomato, cotton, onion
insert into crops (code, name, name_local, category, is_perennial) values
 ('grapes','Grapes','{"mr":"द्राक्ष","hi":"अंगूर"}','fruit',true),
 ('tomato','Tomato','{"mr":"टोमॅटो","hi":"टमाटर"}','vegetable',false),
 ('cotton','Cotton','{"mr":"कापूस","hi":"कपास"}','fibre',false),
 ('onion','Onion','{"mr":"कांदा","hi":"प्याज"}','vegetable',false),
 ('sugarcane','Sugarcane','{"mr":"ऊस","hi":"गन्ना"}','cash_crop',true),
 ('pomegranate','Pomegranate','{"mr":"डाळिंब","hi":"अनार"}','fruit',true),
 ('soybean','Soybean','{"mr":"सोयाबीन","hi":"सोयाबीन"}','oilseed',false),
 ('wheat','Wheat','{"mr":"गहू","hi":"गेहूं"}','cereal',false),
 ('maize','Maize','{"mr":"मका","hi":"मक्का"}','cereal',false),
 ('banana','Banana','{"mr":"केळी","hi":"केला"}','fruit',true)
on conflict (code) do nothing;

insert into crop_varieties (crop_id, code, name)
select c.id, v.code, v.name from crops c join (values
 ('grapes','thompson_seedless','Thompson Seedless'),('grapes','sonaka','Sonaka'),('grapes','sharad_seedless','Sharad Seedless'),
 ('tomato','hybrid_generic','Hybrid (generic)'),('cotton','bt_cotton','Bt Cotton'),('onion','nashik_red','Nashik Red'),
 ('pomegranate','bhagwa','Bhagwa'),('banana','grand_naine','Grand Naine')) as v(crop, code, name) on v.crop = c.code
on conflict (crop_id, code) do nothing;

insert into crop_stages (crop_id, code, name, sequence, typical_duration_days)
select c.id, s.code, s.name, s.seq, s.days from crops c join (values
 ('grapes','dormancy','Dormancy',1,30),('grapes','pruning','Pruning',2,10),('grapes','bud_break','Bud Break',3,15),
 ('grapes','shoot_growth','Shoot Growth',4,30),('grapes','flowering','Flowering',5,15),('grapes','fruit_set','Fruit Set',6,20),
 ('grapes','berry_development','Berry Development',7,40),('grapes','veraison','Veraison / Ripening',8,25),('grapes','harvest','Harvest',9,20),
 ('tomato','nursery','Nursery',1,25),('tomato','transplanting','Transplanting',2,7),('tomato','vegetative','Vegetative',3,25),
 ('tomato','flowering','Flowering',4,20),('tomato','fruiting','Fruiting',5,30),('tomato','harvest','Harvest',6,45),
 ('cotton','sowing','Sowing',1,7),('cotton','germination','Germination',2,10),('cotton','vegetative','Vegetative',3,40),
 ('cotton','squaring','Squaring',4,25),('cotton','flowering','Flowering',5,30),('cotton','boll_development','Boll Development',6,45),
 ('cotton','picking','Picking',7,60),
 ('onion','nursery','Nursery',1,45),('onion','transplanting','Transplanting',2,7),('onion','vegetative','Vegetative',3,35),
 ('onion','bulb_initiation','Bulb Initiation',4,20),('onion','bulb_development','Bulb Development',5,30),('onion','harvest','Harvest',6,15)
) as s(crop, code, name, seq, days) on s.crop = c.code
on conflict (crop_id, code) do nothing;

insert into crop_diseases (crop_id, code, kind, name, name_local)
select c.id, d.code, d.kind, d.name, d.local::jsonb from (values
 ('grapes','grape_downy_mildew','disease','Downy Mildew','{"mr":"केवडा","hi":"डाउनी मिल्ड्यू"}'),
 ('grapes','grape_powdery_mildew','disease','Powdery Mildew','{"mr":"भुरी","hi":"पाउडरी मिल्ड्यू"}'),
 ('grapes','grape_anthracnose','disease','Anthracnose','{"mr":"करपा","hi":"एन्थ्रेक्नोज"}'),
 ('tomato','tomato_early_blight','disease','Early Blight','{"mr":"लवकर येणारा करपा","hi":"अगेती झुलसा"}'),
 ('tomato','tomato_late_blight','disease','Late Blight','{"mr":"उशिरा येणारा करपा","hi":"पछेती झुलसा"}'),
 ('cotton','cotton_pink_bollworm','pest','Pink Bollworm','{"mr":"गुलाबी बोंडअळी","hi":"गुलाबी सुंडी"}'),
 ('cotton','cotton_whitefly','pest','Whitefly','{"mr":"पांढरी माशी","hi":"सफेद मक्खी"}'),
 ('onion','onion_purple_blotch','disease','Purple Blotch','{"mr":"जांभळा करपा","hi":"पर्पल ब्लॉच"}')
) as d(crop, code, kind, name, local) join crops c on c.code = d.crop
on conflict (code) do nothing;
